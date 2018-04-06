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
            answersCount = answers.Count();
        }

        Question question;
        List<Answer> answers;
        List<CheckBox> checkBoxes = new List<CheckBox>();
        int answersCount;

        private void CreateGrid(List<Answer> answers)
        {           
            for (int i = 0; i < answersCount; i++)
            {
                var newRow = new RowDefinition();
                //newRow.Height = new GridLength(60);
                gridAnswers.RowDefinitions.Add(newRow);
            }

            for (int i = 0; i < answersCount; i++)
            {
                CheckBox newCheckBox = new CheckBox
                {
                    Name = $"CheckBox{answers[i].Id}",
                    IsEnabled = true                    
                };

                newCheckBox.Checked += NewCheckBox_Checked;
                newCheckBox.Unchecked += NewCheckBox_Unchecked;

                var tb = new TextBlock
                {
                    Text = $"{answers[i].Text}",
                    FontSize = 15,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextWrapping = TextWrapping.Wrap
                };

                newCheckBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                newCheckBox.Margin = new Thickness(5);
                newCheckBox.Content = tb;

                Grid.SetRow(newCheckBox, i);
                gridAnswers.Children.Add(newCheckBox);
                checkBoxes.Add(newCheckBox);
            }

        }

        private void NewCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (answers.FindAll(a => a.IsCorrect == true).Count() == 1)
                checkBoxes.ForEach(cb => cb.IsEnabled = true);
        }

        private void NewCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (answers.FindAll(a => a.IsCorrect == true).Count() == 1)
                checkBoxes.ForEach(cb => cb.IsEnabled = (bool)cb.IsChecked);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            buttonSubmit.Visibility = Visibility.Hidden;
            buttonExit.Visibility = Visibility.Visible;
            var correctAnswers = answers.FindAll(a => a.IsCorrect == true);
            var chosenAnswers = checkBoxes.FindAll(a => a.IsChecked == true);
            
            if (answers.FindAll(a => a.IsCorrect == true).Count() != 1)
            {
                foreach (var item in checkBoxes)
                {
                    foreach (var ans in correctAnswers)
                    {
                        if ($"CheckBox{ans.Id}" == item.Name)
                        {
                            if (item.IsChecked == true)
                            {
                                var tb = item.Content as TextBlock;
                                tb.Foreground = Brushes.LightGreen;
                                tb.Text += "  - Correct";
                                item.Content = tb;
                                chosenAnswers.Remove(item);
                            }
                            else
                            {
                                var tb = item.Content as TextBlock;
                                tb.Foreground = Brushes.OrangeRed;
                                tb.Text += "  - Correct, Not Chosen";
                                item.Content = tb;
                            }
                        }
                    }
                }

                foreach (var item in chosenAnswers)
                {
                    var tb = item.Content as TextBlock;
                    tb.Foreground = Brushes.OrangeRed;
                    tb.Text += "  - Incorrect";
                    item.Content = tb;
                }

                foreach (var box in checkBoxes)
                {
                    box.IsEnabled = false;
                }
                

            }
            else
            {
                var correctAnswer = answers.FirstOrDefault(a => a.IsCorrect == true);
                Answer chosenAnswer;
                var chosenCB = checkBoxes.FindAll(a => a.IsChecked == true);

                if (chosenCB.Count() > 1)
                    throw new Exception("Something went wrong");
                else                
                    chosenAnswer = answers[int.Parse(chosenCB[0].Name.Substring(8))];
                

                gridAnswers.Children.Clear();
                gridAnswers.RowDefinitions.Clear();

                StackPanel sp = new StackPanel();

                TextBlock tbChose = new TextBlock
                {
                    FontSize = 27
                };
                if (chosenAnswer.Id == correctAnswer.Id)
                    tbChose.Foreground = Brushes.LightGreen;
                else
                    tbChose.Foreground = Brushes.OrangeRed;
                tbChose.Text = "Your choice: ";

                TextBlock tbChoseAns = new TextBlock
                {
                    FontSize = 27,
                    Foreground = Brushes.White,
                    TextWrapping = TextWrapping.Wrap,
                    Text = $"{chosenAnswer.Text}"
                };

                StackPanel spChose = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                spChose.Children.Add(tbChose);
                spChose.Children.Add(tbChoseAns);


                TextBlock tbCorr = new TextBlock
                {
                    FontSize = 27,
                    Text = "Correct answer: ",
                    Foreground = Brushes.LightGreen
                };

                TextBlock tbCorrAns = new TextBlock
                {
                    FontSize = 27,
                    Foreground = Brushes.White,
                    TextWrapping = TextWrapping.Wrap,
                    Text = $"{correctAnswer.Text}"
                };

                StackPanel spCorr = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                spCorr.Children.Add(tbCorr);
                spCorr.Children.Add(tbCorrAns);



                TextBlock expl = new TextBlock
                {
                    FontSize = 27,
                    Foreground = Brushes.White,
                    TextWrapping = TextWrapping.Wrap,
                    Text = $"Explanation: {question.Explanation}"
                };

                sp.Children.Add(spChose);
                sp.Children.Add(spCorr);
                sp.Children.Add(expl);

                gridAnswers.Children.Add(sp);
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
