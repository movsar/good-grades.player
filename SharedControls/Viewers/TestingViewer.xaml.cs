using Data.Entities;
using Data.Entities.TaskItems;
using Shared.Controls;
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
                var questionViewControl = new TestingQuestionViewControl(question);
                spQuestions.Children.Add(questionViewControl);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var questionViewControls = new List<TestingQuestionViewControl>();

            foreach (var view in spQuestions.Children)
            {
                if (view is TestingQuestionViewControl)
                {
                    questionViewControls.Add(view as TestingQuestionViewControl);
                }
            }

            var useSelections = questionViewControls.ToDictionary(qv => (qv.Question.Id), qv => qv.SelectedOptionId);
            foreach (var question in _testingTask.Questions)
            {
                if (useSelections[question.Id] != question.CorrectOptionId)
                {
                    MessageBox.Show("Неправильный ответ(ы)");
                    return;
                }
            }
         
            MessageBox.Show("Ура!");
        }
    }
}
