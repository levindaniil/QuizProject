using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.Model
{
    public class QuestionDB
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string ReportId { get; set; }
        public string ReportTimestamp { get; set; }
        public virtual List<AnswerDB> AnswersDB { get; set; }
        public string ReplyTime { get; set; }
        public int? IsOK { get; set; }
        public int State { get; set; }
    }
}
