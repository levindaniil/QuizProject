using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        Repository<Question> QuestionRepo;
        Repository<Answer> AnswerRepo;

        List<Question> Questions;
        List<Answer> Answers;

        public MainWindow()
        {
            InitializeComponent();            
            QuestionRepo = new QuestionRepository();
            AnswerRepo = new AnswerRepository();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Questions = QuestionRepo.Data.ToList();
            Answers = AnswerRepo.Data.ToList();

            var currentQuestion = Questions.FirstOrDefault(q => q.Date.Date == DateTime.Now.Date);
            var currentAnswers = Answers.FindAll(a => a.Question.Id == currentQuestion.Id).ToList();

            int qtyAnswers = currentAnswers.FindAll(a => a.IsCorrect == true).Count();

            if (qtyAnswers == 1)
                mainFrame.NavigationService.Navigate(new SingleAnswerPage(currentQuestion, currentAnswers));
            else if (qtyAnswers == 0)
                MessageBox.Show("Something went wrong");
            else
                mainFrame.NavigationService.Navigate(new MultipleAnswerPage());

            //listBoxAnswers.ItemsSource = Answers;
            //listBoxQuestions.ItemsSource = Questions;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            // Try this to remove icon from your Taskbar
            //*
            //ShowInTaskbar = false;
            //*
        }
    }
}
