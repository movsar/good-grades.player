using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Shared.Controls;
using Shared.Interfaces;
using Shared.Translations;
using System;
using System.Windows;

namespace Shared.Viewers
{
    public partial class SelectingViewer : Window, IAssignmentViewer
    {
        private readonly SelectingTaskAssignment _assignment;

        public string TaskTitle { get; }

        private readonly Question _question;

        public event Action<IAssignment, bool> CompletionStateChanged;

        public SelectingViewer(SelectingTaskAssignment selectingTask)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = selectingTask;
            TaskTitle = _assignment.Question.Text;
            _question = _assignment.Question;

            var questionViewControl = new QuestionViewControl(_assignment.Question);
            spItems.Children.Add(questionViewControl);
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            var questionViewControls = spItems.Children[0] as QuestionViewControl;

            if (questionViewControls.SelectedOptionId == _question.CorrectOptionId)
            {
                MessageBox.Show(Ru.Correct);
            CompletionStateChanged?.Invoke(_assignment, true);
            }
            else
            {
                MessageBox.Show(Ru.Incorrect);
            CompletionStateChanged?.Invoke(_assignment, false);
            }

        }
    }
}
