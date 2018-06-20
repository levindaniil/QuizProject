using Newtonsoft.Json;
using QuizTool.Logic.DTO;
using QuizTool.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.RESTClient
{
    public class Client
    {
        public Client()
        {

        }

        private const string hostName = "http://localhost:49617";

        public async Task<Question> GetReport(DateTime date)
        {
            string resultContent;

            QuestionReportRequest request = new QuestionReportRequest
            {
                Date = date
            };

            var jsonRequest = JsonConvert.SerializeObject(request);
            HttpResponseMessage result;

            using (var client = new HttpClient())
            {
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(hostName);
                result = client.PostAsync("/api/report/putreport", httpContent).Result;
                resultContent = await result.Content.ReadAsStringAsync();
            }
            try
            {
                var item = JsonConvert.DeserializeObject<IncomingQuestionReport>(resultContent);
                return CreateQuestion(item);
            }
            catch { }
            return null;
        }

        public async Task<State> SubmitReport(Question item)
        {
            HttpStatusCode responseCode;
            string resultContent;

            var request = CreateSubmitRequest(item);

            var jsonRequest = JsonConvert.SerializeObject(request);

            using (var client = new HttpClient())
            {
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(hostName);
                var result = client.PostAsync("/api/userrequest/getreport", httpContent).Result;
                resultContent = await result.Content.ReadAsStringAsync();
                responseCode = result.StatusCode;
            }

            try
            {
                var response = JsonConvert.DeserializeObject<Guid>(resultContent);
                if (response != null && response.GetType().Equals(Guid.Empty.GetType()))
                {
                    return State.Sent;
                }
            }
            catch
            {
                if (responseCode == HttpStatusCode.Conflict)
                    return State.Sent;
            }
            return State.Error;
        }

        private Question CreateQuestion(IncomingQuestionReport item)
        {
            var question = new Question
            {
                Date = item.Question.Date,
                Description = item.Question.Explanation,
                ReportId = item.Id,
                ReportTimestamp = item.Created,
                Text = item.Question.Text,
                Answers = new List<Answer>(),
                State = item.Question.Date == DateTime.Now.Date ? (int)State.Active : (int)State.Idle
            };
            foreach (IncomingAnswer answer in item.Answers)
            {
                question.Answers.Add(new Answer
                {
                    ExternalId = answer.Id,
                    IsCorrect = answer.IsCorrect,
                    Text = answer.Text
                });
            }
            return question;
        }

        private SubmitReportRequest CreateSubmitRequest(Question item)
        {
            if (item.IsOK == null)
            {
                return new SubmitReportRequest
                {
                    IsOK = false,
                    Created = item.ReportTimestamp,
                    Report_Guid = item.ReportId,
                    Finished = null,
                    Answers_Id = new List<int>()
                };
            }
            return new SubmitReportRequest
            {
                IsOK = item.IsOK.GetValueOrDefault(),
                Created = item.ReportTimestamp,
                Report_Guid = item.ReportId,
                Finished = item.ReplyTime.GetValueOrDefault(),
                Answers_Id = item.Answers.Select(a => a.ExternalId).ToList()
            };
        }
    }
}
