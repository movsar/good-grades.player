using Data.Entities;
using Data.Entities.TaskItems;
using Shared.Controls;
using Shared.Translations;
using System.Windows;

namespace Shared.Viewers
{
    public partial class SelectingViewer : Window
    {
        public string TaskTitle { get; }

        private readonly Question _question;

        public SelectingViewer(SelectingTaskAssignment selectingTask)
        {
            InitializeComponent();
            DataContext = this;

            TaskTitle = selectingTask.Question.Text;
            _question = selectingTask.Question;

            var questionViewControl = new TestingQuestionViewControl(selectingTask.Question);
            spItems.Children.Add(questionViewControl);
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            var questionViewControls = spItems.Children[0] as TestingQuestionViewControl;

            if (questionViewControls.SelectedOptionId == _question.CorrectOptionId)
            {
                MessageBox.Show(Ru.Correct);
            }
            else
            {
                MessageBox.Show(Ru.Incorrect);
            }

        }
    }
}
