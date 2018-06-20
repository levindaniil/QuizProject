using QuizTool.Logic.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic
{
    public class Context : DbContext
    {
        public DbSet<QuestionDB> QuestionsDB { get; set; }
        public DbSet<AnswerDB> AnswersDB { get; set; }

        public Context() : base("QuizSQLiteDB")
        {
        }
    }

}

