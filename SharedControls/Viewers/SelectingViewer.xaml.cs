using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Shared.Controls;
using Shared.Interfaces;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Shared.Viewers
{
    public partial class SelectingViewer : Window, IAssignmentViewer
    {
        private readonly SelectingAssignment _assignment;
        private readonly Question _question;

        public string TaskTitle { get; }

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
            var questionViewControl = spQuestion.Children[0] as QuestionViewControl;
            var selectedOptionIds = questionViewControl!.SelectedOptionIds;

            var areAnswersCorrect = QuestionService.CheckAnswersForQuestion(_question, selectedOptionIds);
            var correctOptionIds = QuestionService.GetCorrectOptionIds(_question);

            // Передаём информацию о правильности ответа для подсветки
            questionViewControl.HighlightCorrectOptions(correctOptionIds, areAnswersCorrect);

            if (areAnswersCorrect)
            {
                MessageBox.Show(Translations.GetValue("Correct"), "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(Translations.GetValue("Incorrect"), "Result", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}