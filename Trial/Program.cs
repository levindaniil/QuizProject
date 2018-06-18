using QuizTool.Logic.Model;
using QuizTool.Logic.Repository;
using QuizTool.Logic.RESTClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();

            var task = client.GetReport(DateTime.Now.Date);
            task.Wait();
            var question = task.Result;

            //var questionRepo = RepositoryFactory.Default.GetRepository<Question>();
            //var question = questionRepo.Data.FirstOrDefault(q => q.Date == DateTime.Now.Date);

            //var task = client.SubmitReport(question);
            //task.Wait();
            //var reportId = task.Result;

            Console.WriteLine(question);
            Console.ReadLine();
        }
    }
}
