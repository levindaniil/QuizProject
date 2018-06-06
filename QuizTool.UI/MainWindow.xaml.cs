using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Diagnostics;
using System.IO;
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
using QuizTool.Logic.Migrations;

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

        public MainWindow()
        {
            InitializeComponent();
            QuestionRepo = new QuestionRepository();
            AnswerRepo = new AnswerRepository();
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {        
           
            //listBoxAnswers.ItemsSource = Answers;
            //listBoxQuestions.ItemsSource = Questions;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            var currentQuestion = QuestionRepo.Data.ToList().FirstOrDefault(q => q.Date.Date == DateTime.Now.Date);

            //If statement for checking questions
            if (currentQuestion == null)
            {
                var result = MessageBox.Show("Questions are not found!" + "\n" + "The application will be closed", "Sorry, something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            else
            {
                var currentAnswers = AnswerRepo.Data.ToList().FindAll(a => a.Question.Id == currentQuestion.Id).ToList();

                int qtyAnswers = currentAnswers.FindAll(a => a.IsCorrect == true).Count();

                if (qtyAnswers != 0)
                    mainFrame.NavigationService.Navigate(new MultipleAnswerPage(currentQuestion, currentAnswers));
                else
                {
                    MessageBox.Show("Answers are not found!" + "\n" + "The application will be closed","Something went wrong");
                    Application.Current.Shutdown();
                }
            }
            

            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
        }
    }
}
