using Data.Entities;
using Data.Interfaces;
using Shared.Interfaces;
using Shared.Services;
using Shared.Viewers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls.Assignments
{
    public partial class TestingAssignmentControl : UserControl, IAssignmentViewer
    {
        private readonly TestingAssignment _assignment;
        private int _currentQuestionIndex;
        private List<QuestionViewControl> _questionViewControls;


        public event Action<IAssignment, bool> CompletionStateChanged;

        public TestingAssignmentControl(TestingAssignment testingTask)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = testingTask;
            _currentQuestionIndex = 0;
            _questionViewControls = new List<QuestionViewControl>();

            foreach (var question in _assignment.Questions)
            {
                var questionViewControl = new QuestionViewControl(question);
                _questionViewControls.Add(questionViewControl);
            }

            ShowCurrentQuestion();
        }

        private void ShowCurrentQuestion()
        {
            spQuestions.Children.Clear();
            spQuestions.Children.Add(_questionViewControls[_currentQuestionIndex]);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var questionViewControl = _questionViewControls[_currentQuestionIndex];
            var selections = questionViewControl.SelectedOptionIds;

            _currentQuestionIndex++;

            if (_currentQuestionIndex < _assignment.Questions.Count)
            {
                ShowCurrentQuestion();
            }
            else
            {
                ShowStatistics();
            }


        }

        private void ShowStatistics()
        {
            int correctAnswers = _questionViewControls.Count(qvc =>
                QuestionService.CheckUserAnswers(qvc.Question, qvc.SelectedOptionIds));
            int incorrectAnswers = _assignment.Questions.Count - correctAnswers;

            var statisticsViewer = new StatisticsViewer(correctAnswers, incorrectAnswers);
            statisticsViewer.AssignmentCompleted += (isComplete) =>
            {
                if (isComplete)
                {
                    CompletionStateChanged?.Invoke(_assignment, true);
                }
            };

            statisticsViewer.ShowDialog();

            if (incorrectAnswers > 0)
            {
                RestartTest();
            }
            else
            {
                this.Close();
            }
        }

        private void RestartTest()
        {
            _currentQuestionIndex = 0;
            foreach (var control in _questionViewControls)
            {
                control.ResetSelections();
            }
            ShowCurrentQuestion();
        }
    }
}
}
