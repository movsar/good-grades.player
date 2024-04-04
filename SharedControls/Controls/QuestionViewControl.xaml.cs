using Data.Entities.TaskItems;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class QuestionViewControl : UserControl
    {
        public string SelectedOptionId { get; set; }
        public Question Question { get; }

        public QuestionViewControl(Question testingQuestion)
        {
            InitializeComponent();
            DataContext = this;

            Question = testingQuestion;

            var isMultichoice = Question.Options.Where(o => o.IsChecked).Count() > 1;
            spOptions.Children.Clear();
            foreach (var option in Question.Options)
            {
                var spOption = isMultichoice ? GenerateCheckboxOptionView(option) : GenerateRadioButtonOptionView(option, Question.Id);
                spOptions.Children.Add(spOption);
            }
        }

        private StackPanel GenerateRadioButtonOptionView(AssignmentItem option, string groupName)
        {
            var spOption = new StackPanel()
            {
                Tag = option.Id,
                Orientation = Orientation.Horizontal
            };

            var radioButton = new RadioButton()
            {
                GroupName = groupName,
                Content = option.Text,
                Style = (Style)FindResource("RadioOptionStyle"),
            };

            spOption.Children.Add(radioButton);

            return spOption;
        }

        private StackPanel GenerateCheckboxOptionView(AssignmentItem option)
        {
            var spOption = new StackPanel()
            {
                Tag = option.Id,
                Orientation = Orientation.Horizontal
            };

            var checkbox = new CheckBox()
            {
                Content = option.Text,
                Style = (Style)FindResource("CheckboxOptionStyle"),
            };

            spOption.Children.Add(checkbox);

            return spOption;
        }


        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.DataContext is AssignmentItem)
            {
                SelectedOptionId = ((AssignmentItem)radioButton.DataContext).Id;
            }
        }
    }
}
