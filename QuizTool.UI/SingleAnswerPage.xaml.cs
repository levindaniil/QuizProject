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
    /// Логика взаимодействия для SingleAnswerPage.xaml
    /// </summary>
    public partial class SingleAnswerPage : Page
    {
        Question question;
        List<Answer> answers;
        List<Button> buttons = new List<Button>();        

        public SingleAnswerPage(Question _question, List<Answer> _answers)
        {
            InitializeComponent();
            question = _question;
            answers = _answers;
        }


        private void CreateGrid(List<Answer> answers)
        {
            int c = answers.Count();

            for (int i = 0; i < c; i++)
            {
                var newRow = new RowDefinition();
                newRow.Height = new GridLength(60);
                gridAnswers.RowDefinitions.Add(newRow);                
            }

            for (int i = 0; i < c; i++)
            {
                Button newButton = new Button();
                newButton.Name = $"button{answers[i].Id}";                
                newButton.IsEnabled = true;
                
                newButton.Content = $"  {answers[i].Text}";
                newButton.FontSize = 20;
                newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                newButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                newButton.Margin = new Thickness(5);
                newButton.ToolTip = "Click to answer";
                newButton.Click += NewButton_Click;                
                Grid.SetRow(newButton, i);
                gridAnswers.Children.Add(newButton);
                buttons.Add(newButton);
            }                                                         
               
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            var clicked = sender as Button;
            var chosenAnswer = answers.FirstOrDefault(a => a.Id == int.Parse(clicked.Name.Substring(6)));
            var correctAnswer = answers.FirstOrDefault(a => a.IsCorrect == true);

            gridAnswers.Children.Clear();
            gridAnswers.RowDefinitions.Clear();

            StackPanel sp = new StackPanel();            

            TextBlock tbChose = new TextBlock();
            tbChose.FontSize = 20;
            if (chosenAnswer.Id == correctAnswer.Id)
                tbChose.Foreground = Brushes.LightGreen;
            else
                tbChose.Foreground = Brushes.Red;
            tbChose.Text = "Your choice: ";

            TextBlock tbChoseAns = new TextBlock();
            tbChoseAns.FontSize = 20;
            tbChoseAns.Foreground = Brushes.White;
            tbChoseAns.TextWrapping = TextWrapping.Wrap;
            tbChoseAns.Text = $"{chosenAnswer.Text}";

            StackPanel spChose = new StackPanel();
            spChose.Orientation = Orientation.Horizontal;
            spChose.Children.Add(tbChose);
            spChose.Children.Add(tbChoseAns);


            TextBlock tbCorr = new TextBlock();
            tbCorr.FontSize = 20;
            tbCorr.Text = "Correct answer: ";
            tbCorr.Foreground = Brushes.LightGreen;

            TextBlock tbCorrAns = new TextBlock();
            tbCorrAns.FontSize = 20;
            tbCorrAns.Foreground = Brushes.White;
            tbCorrAns.TextWrapping = TextWrapping.Wrap;
            tbCorrAns.Text = $"{correctAnswer.Text}";

            StackPanel spCorr = new StackPanel();
            spCorr.Orientation = Orientation.Horizontal;
            spCorr.Children.Add(tbCorr);
            spCorr.Children.Add(tbCorrAns);            
            
            

            TextBlock expl = new TextBlock();
            expl.FontSize = 20;
            expl.Foreground = Brushes.White;
            expl.TextWrapping = TextWrapping.Wrap;
            expl.Text = $"Explanation: {question.Explanation}";

            sp.Children.Add(spChose);
            sp.Children.Add(spCorr);
            sp.Children.Add(expl);

            gridAnswers.Children.Add(sp);

            buttonExit.Visibility = Visibility.Visible;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CreateGrid(answers);
            textBoxQuestion.Text = $" {question.Text}";
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
