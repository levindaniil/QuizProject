using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.Model
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Guid ReportId { get; set; }
        public DateTime ReportTimestamp { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public DateTime? ReplyTime { get; set; }
        public bool? IsOK { get; set; }
        public int State { get; set; }

    }
}
