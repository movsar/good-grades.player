using GGManager.Models;
using GGManager.Stores;
using Data;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Shared;
using Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace GGManager.UserControls
{
    public partial class QuestionEditControl : UserControl
    {
        #region Fields
        static string Hint { get; } = Translations.GetValue("SetDescription");
        private readonly ContentStore ContentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        #endregion

        #region Properties and Events
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();
        public Question Question { get; }
        public event Action<Question> Discarded;
        public event Action<Question> Committed;
        public event Action<Question> Updated;
        #endregion

        #region Initialization
        private void SetUiForNewMaterial()
        {
            btnDiscard.Visibility = Visibility.Collapsed;
            btnCommit.Visibility = Visibility.Visible;
        }
        private void SetUiForExistingMaterial()
        {
            btnDiscard.Visibility = Visibility.Visible;
            btnCommit.Visibility = Visibility.Collapsed;
        }
        private void SharedUiInitialization()
        {
            InitializeComponent();
            DataContext = this;
        }
        public QuestionEditControl()
        {
            SharedUiInitialization();
            SetUiForNewMaterial();

            Question = new Question();

            txtQuestion.Text = Hint;

            RedrawOptions();
        }

        public QuestionEditControl(Question question)
        {
            SharedUiInitialization();
            SetUiForExistingMaterial();

            Question = question;

            txtQuestion.Text = Question.Text;

            RedrawOptions();
        }
        #endregion

        private void RedrawOptions()
        {
            spItems.Children.Clear();
            foreach (var option in Question.Options)
            {
                var existingItemControl = new AssignmentItemEditControl(AssignmentType.Test, option);
                existingItemControl.Discarded += OnOptionDiscarded;

                spItems.Children.Add(existingItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(AssignmentType.Test);
            newItemControl.Committed += OnOptionCommitted;
            spItems.Children.Add(newItemControl);
        }
        private void OnOptionCommitted(AssignmentItem optionItem)
        {
            Question.Options.Add(optionItem);
            RedrawOptions();
        }

        private void OnOptionDiscarded(AssignmentItem optionItem)
        {
            Question.Options.Remove(optionItem);
            RedrawOptions();
        }


        #region Event Handlers
        private void txtQuestionText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQuestion.Text == Hint)
            {
                txtQuestion.Text = "";
            }
        }

        private void txtQuestionText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Question.Text))
            {
                txtQuestion.Text = Hint;
            }
        }

        private void txtQuestionText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuestion.Text) || txtQuestion.Text.Equals(Hint))
            {
                return;
            }

            Question.Text = txtQuestion.Text;
        }
        private void btnDiscard_Click(object sender, RoutedEventArgs e)
        {
            Discarded?.Invoke(Question);
        }

        private void txtQuestion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RaiseQuestionCommitEvent();
            }
        }

        #endregion

        private void btnCommit_Click(object sender, RoutedEventArgs e)
        {
            RaiseQuestionCommitEvent();
        }

        private void RaiseQuestionCommitEvent()
        {
            if (string.IsNullOrWhiteSpace(Question.Text))
            {
                MessageBox.Show("Пожалуйста введите текст вопроса");
                return;
            }

            if (Question.Options.Count < 2 || Question.Options.FirstOrDefault(o => o.IsChecked == true) == null)
            {
                MessageBox.Show("Добавьте хотя бы два варианта ответа и хотя бы один выберите как правильный");
                return;
            }

            Committed?.Invoke(Question);
        }
    }
}
