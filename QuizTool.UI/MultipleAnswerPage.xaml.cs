using QuizTool.Logic;
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

namespace QuizTool.UI
{
    /// <summary>
    /// Логика взаимодействия для MultipleAnswerPage.xaml
    /// </summary>
    public partial class MultipleAnswerPage : Page
    {
        public MultipleAnswerPage(Question _question, List<Answer> _answers)
        {
            InitializeComponent();
            question = _question;
            answers = _answers;
        }

        Question question;
        List<Answer> answers;
        List<CheckBox> checkBoxes = new List<CheckBox>();


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
                CheckBox newCheckBox = new CheckBox();
                newCheckBox.Name = $"CheckBox{answers[i].Id}";
                newCheckBox.IsEnabled = true;
                newCheckBox.Content = $"  {answers[i].Text}";
                newCheckBox.FontSize = 30;                
                newCheckBox.HorizontalContentAlignment = HorizontalAlignment.Left;
                newCheckBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                newCheckBox.Margin = new Thickness(5);
                //CheckBox checkBox = new CheckBox();

                Grid.SetRow(newCheckBox, i);
                gridAnswers.Children.Add(newCheckBox);
                checkBoxes.Add(newCheckBox);
            }

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            buttonSubmit.Visibility = Visibility.Hidden;
            buttonExit.Visibility = Visibility.Visible;
            var correctAnswers = answers.FindAll(a => a.IsCorrect == true);
            var chosenAnswers = checkBoxes.FindAll(a => a.IsChecked == true);
            
            foreach (var item in checkBoxes)
            {
                foreach(var ans in correctAnswers)
                {
                    if ($"CheckBox{ans.Id}" == item.Name)
                    {
                        if (item.IsChecked==true)
                        {
                            item.Foreground = Brushes.Green;
                            item.Content += "  - Correct";
                            chosenAnswers.Remove(item);
                        }
                        else { item.Content += "  - Correct, Not Chosen"; }
                    }
                }
            }

            foreach (var item in  chosenAnswers)
            {
                item.Foreground = Brushes.Red;
                item.Content += "  - Incorrect";
            }

            foreach (var box in checkBoxes)
            {
                box.IsEnabled = false;
            }
            buttonSubmit.Visibility = Visibility.Hidden;

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
