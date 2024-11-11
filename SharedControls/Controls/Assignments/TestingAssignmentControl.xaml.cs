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
    public partial class TestingAssignmentControl : UserControl, IAssignmentViewer, INotifyPropertyChanged
    {
        private readonly TestingAssignment _assignment;
        private int _currentQuestionIndex = -1;
        private List<TestingQuestionViewControl> _questionViewControls;

        public event Action<IAssignment, bool> AssignmentCompleted;
        public event Action<IAssignment, string, bool> AssignmentItemSubmitted;
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _taskTitle;
        public string TaskTitle
        {
            get { return _taskTitle; }
            set
            {
                if (_taskTitle != value)
                {
                    _taskTitle = value;
                    OnPropertyChanged(nameof(TaskTitle));
                }
            }
        }
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

            ShowNextQuestion();
        }

        private void ShowNextQuestion()
        {
            _currentQuestionIndex++;
            ccQuestion.Content = _questionViewControls[_currentQuestionIndex];

            var sb = new StringBuilder();
            sb.Append(_currentQuestionIndex + 1);
            sb.Append(" / ");
            sb.Append(_assignment.Questions.Count);
            sb.Append(" ");
            sb.Append(_assignment.Questions[_currentQuestionIndex].Text);
            TaskTitle = sb.ToString();
        }

        private void RestartTest()
        {
            _currentQuestionIndex = -1;
            foreach (var control in _questionViewControls)
            {
                control.ResetSelections();
            }
            ShowNextQuestion();
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
            IsEnabled = true;
            RestartTest();
        }

        public void OnNextClicked()
        {
            IsEnabled = true;

            if (_currentQuestionIndex < _assignment.Questions.Count - 1)
            {
                ShowNextQuestion();
            }
        }
    }
}