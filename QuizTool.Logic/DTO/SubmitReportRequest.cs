using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.DTO
{
    class SubmitReportRequest
    {
        public Guid Report_Guid { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }
        public bool IsOK { get; set; }
        public List<int> Answers_Id { get; set; }
    }
}
