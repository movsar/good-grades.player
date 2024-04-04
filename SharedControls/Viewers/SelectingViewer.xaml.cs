using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Shared.Controls;
using Shared.Interfaces;
using Shared.Translations;
using System;
using System.Linq;
using System.Windows;

namespace Shared.Viewers
{
    public partial class SelectingViewer : Window, IAssignmentViewer
    {
        private readonly SelectingAssignment _assignment;

        public string TaskTitle { get; }

        private readonly Question _question;

        public event Action<IAssignment, bool> CompletionStateChanged;

        public SelectingViewer(SelectingAssignment selectingTask)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = selectingTask;
            TaskTitle = _assignment.Question.Text;
            _question = _assignment.Question;

            var questionViewControl = new QuestionViewControl(_assignment.Question);
            spQuestion.Children.Add(questionViewControl);
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            // Collect user answers
            var questionViewControl = spQuestion.Children[0] as QuestionViewControl;
            var selections = questionViewControl!.SelectedOptionIds;

            // Check user answers
            foreach (var option in _question.Options)
            {
                if (option.IsChecked && !selections.Contains(option.Id))
                {
                    MessageBox.Show(Ru.Incorrect);
                    CompletionStateChanged?.Invoke(_assignment, false);
                    return;
                }
            }

            MessageBox.Show(Ru.Correct);
            CompletionStateChanged?.Invoke(_assignment, true);
        }
    }
}
