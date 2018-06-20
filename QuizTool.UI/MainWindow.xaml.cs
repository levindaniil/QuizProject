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
using System.Threading;
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
using QuizTool.Logic.Model;
using QuizTool.Logic.Repository;

namespace QuizTool.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public bool closedManually = true;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        
        Question currentQuestion;



        public MainWindow()
        {
            InitializeComponent();
            currentQuestion = Manager.DefaultManager.SearchQuestionInDB();
        }      

        private void ClosedProgrammatically()
        {
            closedManually = false;
            Close();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var disp = Application.Current.Dispatcher;

            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            //If statement for checking questions
            if (currentQuestion == null)
            {
                var result = MessageBox.Show("Questions are not found!" + "\n" + "The application will be closed", "QuizTool", MessageBoxButton.OK, MessageBoxImage.Information);
                ClosedProgrammatically();
            }
            else
            {               
                var currentAnswers = currentQuestion.Answers.ToList();

                int qtyAnswers = currentAnswers.FindAll(a => a.IsCorrect == true).Count();

                if (qtyAnswers != 0)
                {
                    var questionPage = new MultipleAnswerPage(currentQuestion, currentAnswers);
                    questionPage.CloseWindow += ClosedProgrammatically;
                    mainFrame.NavigationService.Navigate(questionPage);
                }
                    
                else
                {
                    MessageBox.Show("Answers are not found!" + "\n" + "The application will be closed", "QuizTool", MessageBoxButton.OK, MessageBoxImage.Error);
                    ClosedProgrammatically();
                }
            }
        }

        
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = closedManually;            
            
            if (!closedManually)
                Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
        }
    }
}
