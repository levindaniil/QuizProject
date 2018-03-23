using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuizTool.Logic;

namespace QuizTool.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Repository<Question> QuestionRepo;
        Repository<Answer> AnswerRepo;

        IEnumerable<Question> Questions;
        IEnumerable<Answer> Answers;

        public MainWindow()
        {
            InitializeComponent();
            QuestionRepo = new QuestionRepository();
            AnswerRepo = new AnswerRepository();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Questions = QuestionRepo.Data;
            Answers = AnswerRepo.Data;
            listBoxAnswers.ItemsSource = Answers;
            listBoxQuestions.ItemsSource = Questions;
        }
    }
}
