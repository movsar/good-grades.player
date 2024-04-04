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
using System.Windows.Documents;

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

            var areAnswersCorrect = QuestionService.CheckUserAnswers(_question, selections);
            if (areAnswersCorrect)
            {
                MessageBox.Show(Ru.Correct);
            }
            else
            {
                MessageBox.Show(Ru.Incorrect);
            }

            CompletionStateChanged?.Invoke(_assignment, areAnswersCorrect);
        }
    }
}
