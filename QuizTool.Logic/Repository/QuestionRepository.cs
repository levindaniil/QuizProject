using QuizTool.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.Repository
{
    public class QuestionRepository
    {
        Cast cast = new Cast();

        public Action<Question> ItemAdded { get; set; }

        private List<Question> _items;

        public IEnumerable<Question> Data => _items ?? (_items = new List<Question>());

        public IEnumerable<Question> FindAll(Predicate<Question> predicate)
        {
            return _items.FindAll(predicate);
        }

        public QuestionRepository()
        {
            using (var context = new Context())
            {
                var qDBlist = context.QuestionsDB.Include("AnswersDB").ToList();
                List<Question> qlist = new List<Question>();
                foreach (var qDB in qDBlist)
                {
                    var q = cast.DBToQuestion(qDB);
                    qlist.Add(q);
                }
                _items = qlist;
            }
        }

        public Question AddItem(Question question)
        {
            var questionDB = cast.QuestionToDB(question);
            using (var context = new Context())
            {
                context.QuestionsDB.Add(questionDB);
                context.SaveChanges();
            }

            question = cast.DBToQuestion(questionDB);

            _items.Add(question);
            ItemAdded?.Invoke(question);

            return question;
        }

        public Question ChangeState(Question item, State state)
        {
            QuestionDB questionToEdit;            

            using (var context = new Context())
            {
                questionToEdit = context.QuestionsDB.Include("AnswersDB").FirstOrDefault(q => q.Id == item.Id);
                questionToEdit.State = (int)state;
                context.SaveChanges();                
            }

            _items.FirstOrDefault(q => q.Id == questionToEdit.Id).State = (int)state;
            item.State = (int)state;

            return item;


        }

        public Question EditItem(Question item, List<Answer> answers)
        {

            using (var context = new Context())
            {
                var questionToEdit = context.QuestionsDB.Include("AnswersDB").FirstOrDefault(q => q.Id == item.Id);
                questionToEdit.IsOK = cast.BoolToInt(IsOK(item, answers));
                List<AnswerDB> toDelete = new List<AnswerDB>();
                foreach (AnswerDB answer in questionToEdit.AnswersDB)
                {
                    
                    if (answers.FirstOrDefault(a => a.Id == answer.Id) == null)
                    {
                        toDelete.Add(answer);
                    }
                }
                questionToEdit.AnswersDB.RemoveAll(a => toDelete.Contains(a));
                questionToEdit.ReplyTime = DateTime.Now.ToString();
                questionToEdit.State = (int)State.Answered;
                context.SaveChanges();

                foreach (var ans in toDelete)
                {
                    context.AnswersDB.Remove(context.AnswersDB.FirstOrDefault(a => a.Id == ans.Id));
                }
                context.SaveChanges();

                _items.Remove(item);
                item = cast.DBToQuestion(questionToEdit);
                _items.Add(item);

                return item;
            }
        }
        
        public void RemoveItem(Question question)
        {

            using (var context = new Context())
            {
                var questionToRemove = context.QuestionsDB.Include("AnswersDB").FirstOrDefault(q => q.Id == question.Id);
                context.AnswersDB.RemoveRange(questionToRemove.AnswersDB);
                context.Set<QuestionDB>().Remove(questionToRemove);
                context.SaveChanges();
            }

            _items.Remove(question);
        }

        private bool IsOK(Question question, List<Answer> selected)
        {
            List<Answer> answers = question.Answers.Where(a => a.IsCorrect).ToList();
            if (answers.Count == selected.Count)
            {
                bool flag = true;
                foreach (Answer item in selected)
                {
                    if (!answers.Contains(item))
                    {
                        flag = false;
                        break;
                    }                        
                }
                return flag;
            }
            return false;

        }

    }
}
