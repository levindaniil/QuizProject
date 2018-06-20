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
                _items = context.Questions.Include("Answers").ToList();
            }
        }

        public Question AddItem(Question question)
        {
            using (var context = new Context())
            {
                context.Questions.Add(question);
                context.SaveChanges();
            }

            _items.Add(question);
            ItemAdded?.Invoke(question);
            return question;
        }

        public Question ChangeState(Question item, State state)
        {
            Question questionToEdit;            

            using (var context = new Context())
            {
                questionToEdit = context.Questions.Include("Answers").FirstOrDefault(q => q.Id == item.Id);
                questionToEdit.State = (int)state;
                context.SaveChanges();                
            }

            _items.FirstOrDefault(q => q.Id == questionToEdit.Id).State = (int)state;

            return questionToEdit;


        }

            public Question EditItem(Question item, List<Answer> answers)
        {
            using (var context = new Context())
            {
                var questionToEdit = context.Questions.Include("Answers").FirstOrDefault(q => q.Id == item.Id);
                questionToEdit.IsOK = IsOK(item, answers);
                List<Answer> toDelete = new List<Answer>();
                foreach (Answer answer in questionToEdit.Answers)
                {
                    
                    if (answers.FirstOrDefault(a => a.Id == answer.Id) == null)
                    {
                        toDelete.Add(answer);
                    }
                }
                questionToEdit.Answers.RemoveAll(a => toDelete.Contains(a));
                questionToEdit.ReplyTime = DateTime.Now;
                questionToEdit.State = (int)State.Answered;
                context.SaveChanges();

                foreach (var ans in toDelete)
                {
                    context.Answers.Remove(context.Answers.FirstOrDefault(a => a.Id == ans.Id));
                }
                context.SaveChanges();

                item.IsOK = questionToEdit.IsOK;
                item.Answers = questionToEdit.Answers;
                item.State = questionToEdit.State;
                item.ReplyTime = questionToEdit.ReplyTime;

                return questionToEdit;
            }
        }

        public Question GetItem()
        {
            using (var context = new Context())
            {
                return context.Questions.FirstOrDefault(q => q.Date.Date == DateTime.Now.Date);
            }
        }

        public void RemoveItem(Question question)
        {
            using (var context = new Context())
            {
                var questionToRemove = context.Questions.Include("Answers").FirstOrDefault(q => q.Id == question.Id);
                context.Answers.RemoveRange(questionToRemove.Answers);
                context.Set<Question>().Remove(questionToRemove);
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
