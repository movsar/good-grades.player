using Content_Manager.Models;
using Content_Manager.Stores;
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
using Shared.Translations;
using Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace Content_Manager.UserControls
{
    public partial class QuestionEditControl : UserControl
    {
        #region Fields
        static string Hint { get; } = Ru.SetDescription;
        private FormCompletionInfo _formCompletionInfo;
        private readonly ContentStore ContentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        #endregion

        #region Properties and Events
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();
        public Question Question { get; }
        public event Action<Question> Discarded;
        public event Action<Question> Committed;
        public event Action<Question> Updated;
        #endregion

        #region Reactions
        private void OnTextSet(bool isSet)
        {
            _formCompletionInfo.Update(nameof(Question.Text), isSet);
        }
        #endregion

        #region Initialization
        private void SetUiForNewMaterial()
        {
            btnDeleteQuestion.Visibility = Visibility.Hidden;
            btnSaveQuestion.Visibility = Visibility.Hidden;
        }
        private void SetUiForExistingMaterial()
        {
            btnDeleteQuestion.Visibility = Visibility.Visible;
        }
        private void SharedInitialization(bool isExistingMaterial)
        {
            InitializeComponent();
            DataContext = this;

            var propertiesToWatch = new List<string>
            {
                nameof(QuestionText)
            };

            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingMaterial);
            _formCompletionInfo.StatusChanged += OnFormStatusChanged;
        }
        public QuestionEditControl(TestingAssignment task)
        {
            SharedInitialization(false);
            SetUiForNewMaterial();

            QuestionText = Hint;
            _testingQuestion = new Question();
            _task = task;
        }

        public QuestionEditControl(TestingAssignment task, Question testingQuestion)
        {
            SharedInitialization(true);
            SetUiForExistingMaterial();

            _testingQuestion = testingQuestion;
            _task = task;

            QuestionId = testingQuestion.Id!;
            QuestionText = testingQuestion.Text;
            Options = testingQuestion.Options.ToList();

            foreach (var answer in Options)
            {
                var existingItemControl = new AssignmentItemEditControl(TaskType.Test, answer);
                //existingItemControl.Removed += Option_Delete;

                spItems.Children.Add(existingItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Test);

            spItems.Children.Add(newItemControl);

            OnTextSet(true);
        }


        #endregion

        private Question GetQuestionById(string questionId)
        {
            return ContentStore.SelectedSegment!.TestingTasks.SelectMany(t => t.Questions).Where(q => q.Id == questionId).First();
        }

        #region Event Handlers
      
        private void Option_Create(IEntityBase entity)
        {
            var newAnswer = (AssignmentItem)entity;
            var question = GetQuestionById(QuestionId);

            _testingQuestion.Options.Add(newAnswer);
            ContentStore.DbContext.SaveChanges();

            QuestionUpdated?.Invoke(_testingQuestion);
        }
        private void Option_Save(IEntityBase model)
        {
            QuestionUpdated?.Invoke(_testingQuestion);
        }
        private void Option_Delete(string itemId)
        {
            var item = ContentStore.DbContext.Find<AssignmentItem>(itemId);

            _testingQuestion.Options.Remove(item);
            ContentStore.DbContext.SaveChanges();

            QuestionUpdated?.Invoke(_testingQuestion);
        }
        #endregion

        #region TextHandlers
        private void txtQuestionText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (QuestionText == Hint)
            {
                QuestionText = "";
            }
        }

        private void txtQuestionText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(QuestionText))
            {
                QuestionText = Hint;
            }
        }

        private void txtQuestionText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuestion.Text) || txtQuestion.Text.Equals(Hint))
            {
                OnTextSet(false);
            }
            else
            {
                OnTextSet(true);
            }
        }
        #endregion

        #region Button Handlers
      
        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            QuestionDeleted?.Invoke(QuestionId);
        }
        #endregion

        private void btnDiscard_Click(object sender, RoutedEventArgs e)
        {
            Discarded?.Invoke(Item);
        }
    }
}
