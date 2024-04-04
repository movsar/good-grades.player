using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Shared.Controls;
using Shared.Interfaces;
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
                    questionViewControls.Add(view as QuestionViewControl);
                }
            }

            //var useSelections = questionViewControls.ToDictionary(qv => (qv.Question.Id), qv => qv.SelectedOptionIds);
            //foreach (var question in _assignment.Questions)
            //{
            //    if (useSelections[question.Id] != question.CorrectOptionId)
            //    {
            //        MessageBox.Show(Ru.IncorrectAnswer);
            //        CompletionStateChanged?.Invoke(_assignment, false);
            //        return;
            //    }
            //}

            MessageBox.Show(Ru.Celebrating);
            CompletionStateChanged?.Invoke(_assignment, true);
        }
    }
}
