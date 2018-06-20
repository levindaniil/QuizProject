using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.Model
{
    public class Cast
    {
        public Cast()
        {

        }

        public Answer DBToAnswer(AnswerDB item)
        {
            Answer result = new Answer();
            result.Id = item.Id;
            result.Text = item.Text;
            result.ExternalId = item.ExternalId;
            result.IsCorrect = (bool)IntToBool(item.IsCorrect);

            return result;
        }

        public AnswerDB AnswerToDB(Answer item)
        {
            AnswerDB result = new AnswerDB();
            result.Id = item.Id;
            result.Text = item.Text;
            result.ExternalId = item.ExternalId;
            result.IsCorrect = (int)BoolToInt(item.IsCorrect);

            return result;
        }

        public Question DBToQuestion(QuestionDB item)
        {
            Question result = new Question();

            List<Answer> answers = new List<Answer>();
            foreach (var aDB in item.AnswersDB)
            {
                var a = DBToAnswer(aDB);
                answers.Add(a);
            }

            result.Answers = answers;

            DateTime.TryParse(item.Date, out DateTime date);
            result.Date = date;

            DateTime.TryParse(item.ReportTimestamp, out DateTime timeStamp);
            result.ReportTimestamp = timeStamp;

            DateTime.TryParse(item.ReplyTime, out DateTime replied);
            result.ReplyTime = replied;

            result.IsOK = IntToBool(item.IsOK);

            Guid.TryParse(item.ReportId, out Guid reportId);
            result.ReportId = reportId;

            result.Text = item.Text;

            result.State = item.State;

            result.Description = item.Description;

            result.Id = item.Id;

            return result;

        }

        public QuestionDB QuestionToDB(Question item)
        {
            QuestionDB result = new QuestionDB();

            List<AnswerDB> answers = new List<AnswerDB>();
            foreach (var a in item.Answers)
            {
                var aDB = AnswerToDB(a);
                answers.Add(aDB);
            }

            result.AnswersDB = answers;

            result.Date = item.Date.ToString();

            result.ReportTimestamp = item.ReportTimestamp.ToString();

            result.ReplyTime = item.ReplyTime.ToString();

            result.IsOK = BoolToInt(item.IsOK);

            result.ReportId = item.ReportId.ToString();

            result.Text = item.Text;

            result.State = item.State;

            result.Description = item.Description;

            result.Id = item.Id;

            return result;
        }

        public bool? IntToBool(int? item)
        {
            if (item == 1)
                return true;
            else if (item == 0)
                return false;
            else
                return null;
        }

        public int? BoolToInt(bool? item)
        {
            if (item == true)
                return 1;
            else if (item == false)
                return 0;
            else
                return null;
        }


    }
}
