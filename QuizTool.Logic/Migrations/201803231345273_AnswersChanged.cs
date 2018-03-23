namespace QuizTool.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnswersChanged : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Answers", name: "QuestionId_Id", newName: "Question_Id");
            RenameIndex(table: "dbo.Answers", name: "IX_QuestionId_Id", newName: "IX_Question_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Answers", name: "IX_Question_Id", newName: "IX_QuestionId_Id");
            RenameColumn(table: "dbo.Answers", name: "Question_Id", newName: "QuestionId_Id");
        }
    }
}
