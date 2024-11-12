using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class SelectionAssignmentControl : UserControl, IAssignmentViewer
    {
        private readonly SelectingAssignment _assignment;
        private readonly Question _question;
        public int StepsCount { get; set; } = 1;

        public event Action<IAssignment, bool>? AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;

        public SelectionAssignmentControl(SelectingAssignment selectingTask)
        {
            InitializeComponent();

            DataContext = this;

            _assignment = selectingTask;
            _question = _assignment.Question;
            tbTitle.Text = _assignment.Question.Text;

            var questionViewControl = new SelectingQuestionViewControl(_assignment.Question);
            ccQuestion.Content = questionViewControl;
        }

        public void OnCheckClicked()
        {
            var questionViewControl = ccQuestion.Content as SelectingQuestionViewControl;
            var selections = questionViewControl!.SelectedOptionIds;

            var areAnswersCorrect = QuestionService.CheckUserAnswers(_question, selections);
            var correctOptionIds = QuestionService.GetCorrectOptionIds(_question);

            questionViewControl.HighlightCorrectOptions(correctOptionIds);

            // Проверяем, есть ли выбранный правильный ответ без лишних
            if (areAnswersCorrect && selections.Count == correctOptionIds.Count)
            {
                AssignmentCompleted?.Invoke(_assignment, true);
            }
            else
            {
                AssignmentCompleted?.Invoke(_assignment, false);
            }

            IsEnabled = false;
        }

        public void OnRetryClicked()
        {
            IsEnabled = true;
        }

        public void OnNextClicked()
        {
            IsEnabled = true;
        }
    }
}
