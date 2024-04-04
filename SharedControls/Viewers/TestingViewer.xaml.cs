using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Shared.Controls;
using Shared.Interfaces;
using Shared.Services;
using Shared.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Shared.Viewers
{
    public partial class TestingViewer : Window, IAssignmentViewer
    {
        private readonly TestingAssignment _assignment;

        public TestingViewer(TestingAssignment testingTask)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = testingTask;
            foreach (var question in _assignment.Questions)
            {
                var questionViewControl = new QuestionViewControl(question);
                spQuestions.Children.Add(questionViewControl);
            }
        }

        public event Action<IAssignment, bool> CompletionStateChanged;

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            var questionViewControls = new List<QuestionViewControl>();

            foreach (var view in spQuestions.Children)
            {
                if (view is QuestionViewControl)
                {
                    questionViewControls.Add((QuestionViewControl)view);
                }
            }

            foreach (var questionViewControl in questionViewControls)
            {
                var selections = questionViewControl!.SelectedOptionIds;
                var areAnswersCorrect = QuestionService.CheckUserAnswers(questionViewControl.Question, selections);

                if (!areAnswersCorrect)
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
