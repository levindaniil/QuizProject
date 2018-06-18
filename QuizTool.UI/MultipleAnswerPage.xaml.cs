using QuizTool.Logic;
using QuizTool.Logic.Model;
using QuizTool.Logic.Repository;
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
        QuestionRepository questionRepo = RepositoryFactory.Default.GetRepository<Question>(); 

        public MultipleAnswerPage(Question _question, List<Answer> _answers)
        {
            InitializeComponent();
            question = _question;
            answers = _answers;
            answersCount = answers.Count();
            correctAnswersCount = answers.FindAll(a => a.IsCorrect == true).Count();
        }

        Question question;
        List<Answer> answers;
        List<CheckBox> checkBoxes = new List<CheckBox>();
        List<RadioButton> radioButtons = new List<RadioButton>();
        int answersCount;
        int correctAnswersCount;

        private void CreateGrid(List<Answer> answers)
        {
            if (correctAnswersCount == 0)
            {
                MessageBox.Show("Right answers are not found!" + "\n" + "The application will be closed", "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            else
            {
                for (int i = 0; i < answersCount; i++)
                {
                    var newRow = new RowDefinition();
                    //newRow.Height = new GridLength(60);
                    gridAnswers.RowDefinitions.Add(newRow);
                }

                if (correctAnswersCount == 1)
                {
                    for (int i = 0; i < answersCount; i++)
                    {
                        RadioButton newRadioButton = new RadioButton
                        {
                            Name = $"RadioButton{answers[i].Id}"
                        };

                        newRadioButton.Checked += NewRadioButton_Checked;

                        var tb = new TextBlock
                        {
                            Text = $"{answers[i].Text}",
                            FontSize = 15,
                            VerticalAlignment = VerticalAlignment.Center,
                            Foreground = Brushes.White,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            TextWrapping = TextWrapping.Wrap
                        };

                        newRadioButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                        newRadioButton.Margin = new Thickness(5);
                        newRadioButton.Content = tb;

                        Grid.SetRow(newRadioButton, i);
                        gridAnswers.Children.Add(newRadioButton);
                        radioButtons.Add(newRadioButton);
                    }
                }

                else
                {
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
            }

        }

        private void NewRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            buttonSubmit.IsEnabled = true;
        }

        private void NewCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (var ch in checkBoxes)
                if (ch.IsChecked == true) i++;
            if (i == 0) buttonSubmit.IsEnabled = false;
            else buttonSubmit.IsEnabled = true;
            if (answers.FindAll(a => a.IsCorrect == true).Count() == 1)
                checkBoxes.ForEach(cb => cb.IsEnabled = true);
        }

        private void NewCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            buttonSubmit.IsEnabled = true;
            if (answers.FindAll(a => a.IsCorrect == true).Count() == 1)
                checkBoxes.ForEach(cb => cb.IsEnabled = (bool)cb.IsChecked);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            buttonSubmit.Visibility = Visibility.Hidden;
            buttonExit.Visibility = Visibility.Visible;
                       
            
            if (correctAnswersCount != 1)
            {
                var correctAnswers = answers.FindAll(a => a.IsCorrect == true);
                var chosenAnswers = checkBoxes.FindAll(a => a.IsChecked == true);
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

                List<int> userAnswersNums = chosenAnswers.Select(c => int.Parse(c.Name.Substring(11))).ToList();
                List<Answer> userAnswers = new List<Answer>();
                foreach (var id in userAnswersNums)
                {
                    userAnswers.Add(answers.FirstOrDefault(a => a.Id == id));
                }
                questionRepo.EditItem(question, userAnswers);

            }
            else
            {
                var correctAnswer = answers.FirstOrDefault(a => a.IsCorrect == true);
                Answer chosenAnswer;
                var chosenCB = radioButtons.FindAll(a => a.IsChecked == true);
                
                chosenAnswer = answers.FirstOrDefault(a => a.Id == int.Parse(chosenCB[0].Name.Substring(11)));

                List<Answer> userAnswer = new List<Answer>()
                {
                    chosenAnswer
                };
                questionRepo.EditItem(question, userAnswer);


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
                    Text = $"Explanation: {question.Description}"
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
