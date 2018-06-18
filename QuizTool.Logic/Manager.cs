using QuizTool.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic
{
    public class Manager
    {
        Action<Question> questionFound;
        Action questionNotFound;
        Action questionSubmited;

        public Manager()
        {
            //Создаешь правила загрузки и отправки объектов
            //Запускаешь их на выполнение
            //
        }

        public void SearchForQuestion()
        {
            //Ищешь вопрос
            //Если есть -- дергаешь questionFound
            //Если нет -- запускаешь повторение попыток, раз 5-10 с интервалом в 5-10 сек.
            //Если все еще нет -- дершаешь questionNotFound
        }

        public void SubmitQuestion(Question question, List<Answer> answers)
        {
            //Делаешь апдейт
            //Дергаешь questionSubmitted (прячешь окно)
            //Ждешь 1 минуту, если все со статусом Error, ничего со статусом Answered нет -- закрываешь приложение
            //Все, что превращается в sent -- удаляешь из бд
        }
    }
}
