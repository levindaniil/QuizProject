using QuizTool.Logic.Model;
using QuizTool.Logic.Repository;
using QuizTool.Logic.RESTClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuizTool.Logic
{

    public class Manager
    {
        public static Manager DefaultManager = new Manager();

        public Action WorkFinished;
        public Action FinishWork;

        public bool Finished { get; set; }
        public bool UIFinished { get; set; }

        QuestionRepository questionRepo = RepositoryFactory.Default.GetRepository<Question>();
        Client client = new Client();

        //Action<Question> questionFound;
        //Action questionNotFound;
        //Action questionSubmited;

        public Manager()
        {
            //Создаешь правила загрузки и отправки объектов
            //Запускаешь их на выполнение
            Finished = false;
            UIFinished = false;
            UpdateQuestion();
            FinishWork += FinishAction;
        }

        public void UpdateQuestion()
        {
            var question = questionRepo.Data.FirstOrDefault(q => q.Date.Date == DateTime.Now.Date && q.State == (int)State.Idle);
            if (question!= null)
            {
                questionRepo.ChangeState(question, State.Active);
            }
        }

        public Question SearchQuestionInDB()
        {
            var currentQuestion = questionRepo.Data.ToList().FirstOrDefault(q => q.Date.Date == DateTime.Now.Date && q.State == (int)State.Active);
            return currentQuestion;
        }

        public Question GetQuestionForToday()
        {
            Question newQuestion = null;
            var attempts = 0;
            while (attempts < 5)
            {
                try
                {
                    var task = client.GetReport(DateTime.Now.Date);
                    task.Wait();
                    newQuestion = task.Result;
                    attempts = 5;
                }
                catch
                {
                    attempts++;
                }
            }

            if (newQuestion == null)
                return null;
            else
            {
                questionRepo.AddItem(newQuestion);
                return newQuestion;
            }
        }

        public void GetNextQuestions()
        {
            for (int i = 0; i < 5; i++)
            {
                var date = DateTime.Now.AddDays(i+1).Date;
                var question = questionRepo.Data.FirstOrDefault(q => q.Date.Date == date);
                if (question == null)
                {
                    try
                    {
                        var task = client.GetReport(date);
                        task.Wait();
                        question = task.Result;
                        if (question != null)
                            questionRepo.AddItem(question);
                    }
                    catch { }                
                }
                
            }
        }

        public void SubmitQuestions()
        {
            var date = DateTime.Now.AddDays(-5).Date;
            var questionsToSend = questionRepo.FindAll(q => q.State == (int)State.Answered || (q.State == (int)State.Error && q.Date.Date > date));

            foreach (var q in questionsToSend)
            {
                var attempts = 0;
                var state = (State)q.State;
                while (attempts < 5)
                {
                    try
                    {
                        var task = client.SubmitReport(q);
                        task.Wait();
                        state = task.Result;
                        attempts = 5;
                    }
                    catch
                    {
                        attempts++;
                    }
                }
                questionRepo.ChangeState(q, state);
            }
            

            ClearQuestions();

            //WorkFinished?.Invoke();
        }

        private void ClearQuestions()
        {
            var date = DateTime.Now.AddDays(-5).Date;
            var questionsToDelete = questionRepo.FindAll(q => (q.State == (int)State.Sent || q.State == (int)State.Idle || q.State == (int)State.Error) && q.Date.Date <= date);
            foreach (var q in questionsToDelete)
            {
                questionRepo.RemoveItem(q);
            }
        }


        private void FinishAction()
        {
            GetNextQuestions();
            SubmitQuestions();
            Finished = true;
        }

        private void Finish()
        {            
            
        }
    }
}
