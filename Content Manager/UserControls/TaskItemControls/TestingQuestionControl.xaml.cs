using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls
{
    public partial class TestingQuestionControl : UserControl
    {
        #region Events
        public event Action Refresh;
        public event Action<IEntityBase> Create;
        public event Action<string?, IEntityBase> Update;
        public event Action<string> Delete;
        #endregion

        #region Fields
        private const string Hint = "Введите описание";
        private FormCompletionInfo _formCompletionInfo;
        #endregion

        #region Properties
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();
        private List<AssignmentItem> Answers { get; set; } = new List<AssignmentItem>();
        public string QuestionText
        {
            get { return (string)GetValue(ItemTextProperty); }
            set { SetValue(ItemTextProperty, value); }
        }
        public static readonly DependencyProperty ItemTextProperty =
            DependencyProperty.Register("QuestionText", typeof(string), typeof(TestingQuestionControl), new PropertyMetadata(""));


        public string QuestionId { get; }

        #endregion

        #region Reactions
        private void OnFormStatusChanged(bool isReady)
        {
            if (isReady)
            {
                btnSaveQuestion.Visibility = Visibility.Visible;
            }
            else
            {
                btnSaveQuestion.Visibility = Visibility.Collapsed;
            }
        }
        private void OnTextSet(bool isSet)
        {
            _formCompletionInfo.Update(nameof(QuestionText), isSet);
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

            var propertiesToWatch = new List<string>();
            propertiesToWatch.Add(nameof(QuestionText));

            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingMaterial);
            _formCompletionInfo.StatusChanged += OnFormStatusChanged;
        }
        public TestingQuestionControl()
        {
            SharedInitialization(false);
            SetUiForNewMaterial();

            QuestionText = Hint;
        }

        public TestingQuestionControl(TestingQuestion testingQuestion)
        {
            SharedInitialization(true);
            SetUiForExistingMaterial();

            QuestionId = testingQuestion.Id!;
            QuestionText = testingQuestion.QuestionText;
            Answers = testingQuestion.Answers.ToList();

            //foreach (var answer in Answers)
            //{
            //    var isSelected = _contentStore.SelectedSegment?.TestingTasks.SelectMany(t => t.Questions).Where(q => q.Id == QuestionId).FirstOrDefault()?.CorrectAnswerId == answer.Id;

            //    var existingQuizItemControl = new TextQuizItem(){
            //        Text = answer, isSelected);
            //    existingQuizItemControl.Update += Question_QuizItem_Save;
            //    existingQuizItemControl.Delete += Question_QuizItem_Delete;
            //    existingQuizItemControl.SetAsCorrect += Question_QuizItem_SetAsCorrect;

            //    spItems.Children.Add(existingQuizItemControl);
            //}

            //var newQuizItemControl = new QuizItemControl(QuizTypes.Testing);
            //   newQuizItemControl.Create += Question_QuizItemControl_Create;

            // spItems.Children.Add(newQuizItemControl);

            OnTextSet(true);
        }


        #endregion

        #region Event Handlers
        private void Question_QuizItem_SetAsCorrect(string itemId)
        {
            //var question = _contentStore.SelectedSegment?.TestingQuizes.Questions.Where(q => q.Id == QuestionId).First();
            //question!.CorrectQuizId = itemId;
            //Update?.Invoke(QuestionId, question);
        }
        private void Question_QuizItemControl_Create(IEntityBase model)
        {
            //    var question = _contentStore.GetQuestionById(QuestionId);

            //    _contentStore.CreateSelectableQuizItem(QuizTypes.Testing, model as TextAndImageQuizItem, question);
            Refresh?.Invoke();
        }
        private void Question_QuizItem_Save(string? id, IEntityBase model)
        {
            //Update?.Invoke(QuestionId, _contentStore.GetQuestionById(QuestionId));
        }
        private void Question_QuizItem_Delete(string itemId)
        {
            //var question = _contentStore.GetQuestionById(QuestionId);

            //_contentStore.DeleteSelectableQuizItem(QuizTypes.Testing, itemId, question);
            //Refresh?.Invoke();
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
        private void btnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(QuestionId))
            //{
            //    Create?.Invoke(new TestingQuestion(QuestionText, Answers));
            //}
            //else
            //{
            //    var question = _contentStore.GetQuestionById(QuestionId);
            //    question.QuestionText = QuestionText;
            //    question.QuizItems = Answers;

            //    Update?.Invoke(QuestionId, question);
            //}
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(QuestionId);
        }
        #endregion
    }
}
