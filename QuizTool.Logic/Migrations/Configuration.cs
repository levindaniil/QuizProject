namespace QuizTool.Logic.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<QuizTool.Logic.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QuizTool.Logic.Context context)
        {
            //This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //Question[] qs = new Question[]
            //{
            //    new Question
            //    {
            //        Text = "Are you a bird?",
            //        Explanation = "I think you are not a bird because you can't do chic-chiric",
            //        Date = DateTime.Now.Date
            //    },
            //    new Question
            //    {
            //        Text = "Are you an elephant?",
            //        Explanation = "I think you are not a elephant because you can jump",
            //        Date = DateTime.Now.Date
            //    }
            //};

            //context.Questions.AddOrUpdate(q => q.Id, qs);



            //Answer[] answers = new Answer[]
            //{
            //    new Answer
            //    {
            //        Question = context.Questions.FirstOrDefault(q => q.Id == 13),
            //        Text = "I am a bird",
            //        IsCorrect = false
            //    },
            //    new Answer
            //    {
            //        Question = context.Questions.FirstOrDefault(q => q.Id == 13),
            //        Text = "I am not a bird",
            //        IsCorrect = true
            //    },
            //    new Answer
            //    {
            //        Question = context.Questions.FirstOrDefault(q => q.Id == 13),
            //        Text = "What is going on here?",
            //        IsCorrect = false
            //    },
            //    new Answer
            //    {
            //        Question = context.Questions.FirstOrDefault(q => q.Id == 14),
            //        Text = "I am an elephant",
            //        IsCorrect = false
            //    },
            //    new Answer
            //    {
            //        Question = context.Questions.FirstOrDefault(q => q.Id == 14),
            //        Text = "I am not an elephant",
            //        IsCorrect = true
            //    },
            //    new Answer
            //    {
            //        Question = context.Questions.FirstOrDefault(q => q.Id == 14),
            //        Text = "I am a human being",
            //        IsCorrect = true
            //    },
            //    new Answer
            //    {
            //        Question = context.Questions.FirstOrDefault(q => q.Id == 14),
            //        Text = "I still dont know whats going on here",
            //        IsCorrect = false
            //    }
            //};

            //context.Answers.AddOrUpdate(a => a.Id, answers);
        }
    }
}
