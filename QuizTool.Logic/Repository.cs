﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic
{
    public class Repository<T> : IRepository<T>
    {
        protected List<T> _items;

        public IEnumerable<T> Data => _items ?? (_items = new List<T>());

        public IEnumerable<T> FindAll(Predicate<T> predicate)
        {
            return _items.FindAll(predicate);
        }
    }

    public class QuestionRepository : Repository<Question>
    {
        public QuestionRepository()
        {
            using (var context = new Context())
            {
                _items = context.Questions.ToList();
            }
        }
    }

    public class AnswerRepository : Repository<Answer>
    {
        public AnswerRepository()
        {
            using (var context = new Context())
            {
                _items = context.Answers.ToList();
            }
        }
    }
}
