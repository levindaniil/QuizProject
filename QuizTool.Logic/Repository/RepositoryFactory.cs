using QuizTool.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic.Repository
{
    public class RepositoryFactory
    {
        private RepositoryFactory() { }

        private static RepositoryFactory _default;

        public static RepositoryFactory Default
        {
            get
            {
                if (_default == null)
                    _default = new RepositoryFactory();
                return _default;
            }
        }
                
        private QuestionRepository QuestionRepo;
        
        public QuestionRepository GetRepository<T>()
        {
            if (typeof(T) == typeof(Question))
                return QuestionRepo ?? ((QuestionRepo = new QuestionRepository()));
            else
                throw new Exception("No repository");
        }
    }
}
