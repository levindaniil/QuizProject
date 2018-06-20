using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.Model
{
    public class AnswerDB
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Text { get; set; }
        public int IsCorrect { get; set; }
    }
}
