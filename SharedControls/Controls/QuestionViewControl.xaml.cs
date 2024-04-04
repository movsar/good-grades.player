﻿using Data.Entities.TaskItems;
using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class QuestionViewControl : UserControl
    {
        public string SelectedOptionId { get; set; }
        public Question Question { get; }

        public QuestionViewControl(Question testingQuestion)
        {
            InitializeComponent();
            DataContext = this;

            Question = testingQuestion;
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.DataContext is AssignmentItem)
            {
                SelectedOptionId = ((AssignmentItem)radioButton.DataContext).Id;
            }
        }
    }
}
