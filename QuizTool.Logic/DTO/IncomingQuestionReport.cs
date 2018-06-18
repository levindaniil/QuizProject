using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.DTO
{
    class IncomingQuestionReport
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public IncomingQuestion Question { get; set; }
        public List<IncomingAnswer> Answers { get; set; }
    }
}
