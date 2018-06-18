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
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public Context() : base("QuizDB")
        {
            Database.SetInitializer(
              new DropCreateDatabaseIfModelChanges<Context>());
        }
    }

}

