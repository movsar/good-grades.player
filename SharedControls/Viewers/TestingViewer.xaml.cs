using Data.Entities;
using Data.Entities.TaskItems;
using Shared.Controls;
using Shared.Translations;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Shared.Viewers
{
    /// <summary>
    /// Interaction logic for TestingViewer.xaml
    /// </summary>
    public partial class TestingViewer : Window
    {
        private readonly TestingTaskAssignment _testingTask;

        public TestingViewer(TestingTaskAssignment testingTask)
        {
            InitializeComponent();
            DataContext = this;

            _testingTask = testingTask;

            foreach (var question in testingTask.Questions)
            {
                var questionViewControl = new QuestionViewControl(question);
                spQuestions.Children.Add(questionViewControl);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var questionViewControls = new List<QuestionViewControl>();

            foreach (var view in spQuestions.Children)
            {
                if (view is QuestionViewControl)
                {
                    questionViewControls.Add(view as QuestionViewControl);
                }
            }

            var useSelections = questionViewControls.ToDictionary(qv => (qv.Question.Id), qv => qv.SelectedOptionId);
            foreach (var question in _testingTask.Questions)
            {
                if (useSelections[question.Id] != question.CorrectOptionId)
                {
                    MessageBox.Show(Ru.IncorrectAnswer);
                    return;
                }
            }
         
            MessageBox.Show(Ru.Celebrating);
        }
    }
}
