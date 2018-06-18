using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.DTO
{
    class QuestionReportRequest
    {
        public string User => Environment.UserName;

        public DateTime Date { get; set; }
    }
}
