using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class TestingAssignmentControl : UserControl, IAssignmentViewer
    {
        private readonly TestingAssignment _assignment;
        private int _currentQuestionIndex = -1;
        private List<TestingQuestionViewControl> _questionViewControls;

        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;
        public event PropertyChangedEventHandler? PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CorrectAnswers { get; private set; }
        public int IncorrectAnswers { get; private set; }
        public int StepsCount { get; }

        public TestingAssignmentControl(TestingAssignment testingTask)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = testingTask;
            StepsCount = _assignment.Questions.Count;
            _questionViewControls = new List<TestingQuestionViewControl>();

            foreach (var question in _assignment.Questions)
            {
                var questionViewControl = new TestingQuestionViewControl(question);
                _questionViewControls.Add(questionViewControl);
            }

            LoadNextItem();
        }

        private void LoadNextItem()
        {
            ccQuestion.Content = _questionViewControls[++_currentQuestionIndex];

            tbTitle.Text = GetCurrentQuestionTitle();
        }

        private void LoadPreviousItem()
        {
            ccQuestion.Content = _questionViewControls[--_currentQuestionIndex];

            tbTitle.Text = GetCurrentQuestionTitle();
        }

        private string GetCurrentQuestionTitle()
        {
            var sb = new StringBuilder();
            sb.Append(_currentQuestionIndex + 1);
            sb.Append(" / ");
            sb.Append(_assignment.Questions.Count);
            sb.Append(" ");
            sb.Append(_assignment.Questions[_currentQuestionIndex].Text);
            return sb.ToString();
        }

        private void RestartTest()
        {
            _currentQuestionIndex = -1;
            foreach (var control in _questionViewControls)
            {
                control.ResetSelections();
            }
            LoadNextItem();
        }

        public void OnCheckClicked()
        {
            var questionViewControl = _questionViewControls[_currentQuestionIndex];
            var selections = questionViewControl.SelectedOptionIds;

            CorrectAnswers = _questionViewControls.Count(qvc => QuestionService.CheckUserAnswers(qvc.Question, qvc.SelectedOptionIds));
            IncorrectAnswers = _assignment.Questions.Count - CorrectAnswers;

            AssignmentCompleted?.Invoke(_assignment, IncorrectAnswers == 0);

            IsEnabled = false;
        }

        public void OnRetryClicked()
        {
            RestartTest();
            IsEnabled = true;
        }

        public void OnNextClicked()
        {
            LoadNextItem();
            IsEnabled = true;
        }

        public void OnPreviousClicked()
        {
            LoadPreviousItem();
            IsEnabled = true;
        }
    }
}