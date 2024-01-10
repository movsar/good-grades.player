using Data.Entities.TaskItems;
using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class TestingQuestionViewControl : UserControl
    {
        public string SelectedOptionId { get; set; }
        public Question Question { get; }

        public TestingQuestionViewControl(Question testingQuestion)
        {
            InitializeComponent();
            DataContext = this;

            Question = testingQuestion;
        }

        // Just for the DesignContext to work
        public TestingQuestionViewControl() { }

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
