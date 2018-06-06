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

            Question[] qs = new Question[]
            {
                new Question
                {
                    Text = "Are you a bird?",
                    Explanation = "I think you are not a bird because you can't do chic-chiric",
                    Date = DateTime.Now.Date
                },
                new Question
                {
                    Text = "Are you an elephant?",
                    Explanation = "I think you are not a elephant because you can jump",
                    Date = DateTime.Now.Date.AddDays(1)
                }
            };

            context.Questions.AddOrUpdate(q => q.Id, qs);
            context.SaveChanges();

            int id1 = qs[0].Id;
            int id2 = qs[1].Id;

            Answer[] answers = new Answer[]
            {
                new Answer
                {
                    Question = context.Questions.FirstOrDefault(q => q.Id == id1),
                    Text = "I am a bird",
                    IsCorrect = false
                },
                new Answer
                {
                    Question = context.Questions.FirstOrDefault(q => q.Id == id1),
                    Text = "I am not a bird",
                    IsCorrect = true
                },
                new Answer
                {
                    Question = context.Questions.FirstOrDefault(q => q.Id == id1),
                    Text = "What is going on here?",
                    IsCorrect = false
                },
                new Answer
                {
                    Question = context.Questions.FirstOrDefault(q => q.Id ==  id2),
                    Text = "I am an elephant",
                    IsCorrect = false
                },
                new Answer
                {
                    Question = context.Questions.FirstOrDefault(q => q.Id == id2),
                    Text = "I am not an elephant",
                    IsCorrect = true
                },
                new Answer
                {
                    Question = context.Questions.FirstOrDefault(q => q.Id == id2),
                    Text = "I am a human being",
                    IsCorrect = true
                },
                new Answer
                {
                    Question = context.Questions.FirstOrDefault(q => q.Id == id2),
                    Text = "I still dont know whats going on here",
                    IsCorrect = false
                }
            };

            context.Answers.AddOrUpdate(a => a.Id, answers);

            context.SaveChanges();
        }
    }
}
