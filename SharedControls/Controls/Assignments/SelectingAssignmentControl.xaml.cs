using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class SelectingAssignmentControl : UserControl, IAssignmentControl
    {
        public int StepsCount { get; set; } = 1;
        private SelectingAssignment _assignment;
        private Question _question;
        public event Action<IAssignment, bool>? AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;

        public SelectingAssignmentControl()
        {
            InitializeComponent();
            DataContext = this;
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

        public void OnPreviousClicked()
        {
            IsEnabled = true;
        }

        public void Initialize(IAssignment assignment)
        {
            IsEnabled = true;
            if (_assignment == assignment)
            {
                return;
            }

            _assignment = (SelectingAssignment)assignment;
            _question = _assignment.Question;

            var questionViewControl = new SelectingQuestionViewControl(_question);
            ccQuestion.Content = questionViewControl;
        }
    }
}
