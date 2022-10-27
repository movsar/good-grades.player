using Content_Manager.Interfaces;
using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Content_Manager.UserControls.MaterialControls
{
    public partial class QuestionControl : UserControl, IMaterialControl
    {
        #region Events
        public event Action<string?, IModelBase> Save;
        public event Action<string> Delete;
        #endregion

        #region Fields
        private const string Hint = "Введите описание";
        private FormCompletionInfo _formCompletionInfo;
        private QuizTypes _quizType;
        #endregion

        #region Properties
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();
        private List<QuizItem> QuizItems { get; set; } = new List<QuizItem>();
        public string QuestionText
        {
            get { return (string)GetValue(ItemTextProperty); }
            set { SetValue(ItemTextProperty, value); }
        }
        public static readonly DependencyProperty ItemTextProperty =
            DependencyProperty.Register("QuestionText", typeof(string), typeof(QuestionControl), new PropertyMetadata(""));


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
        public QuestionControl()
        {
            SharedInitialization(false);
            SetUiForNewMaterial();

            QuestionText = Hint;
        }

        public QuestionControl(TestingQuestion testingQuestion)
        {
            SharedInitialization(true);
            SetUiForExistingMaterial();

            QuestionId = testingQuestion.Id!;
            QuestionText = testingQuestion.QuestionText;
            QuizItems = testingQuestion.QuizItems;

            foreach (var quizItem in QuizItems)
            {
                var isSelected = _contentStore.SelectedSegment?.TestingQuiz.Questions.Where(q => q.Id == QuestionId).FirstOrDefault()?.CorrectQuizId == quizItem.Id;

                var existingQuizItemControl = new QuizItemControl(QuizTypes.Testing, quizItem, isSelected);
                existingQuizItemControl.Save += QuizItemControl_Save;
                existingQuizItemControl.Delete += QuizItemControl_Delete;
                existingQuizItemControl.SetAsCorrect += QuizItem_SetAsCorrect;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newQuizItemControl = new QuizItemControl(QuizTypes.Testing);
            newQuizItemControl.Save += QuizItemControl_Save;
            newQuizItemControl.Delete += QuizItemControl_Delete;

            spItems.Children.Add(newQuizItemControl);

            OnTextSet(true);
        }
        #endregion

        #region Event Handlers
        private void QuizItem_SetAsCorrect(string itemId)
        {
            var question = _contentStore.SelectedSegment?.TestingQuiz.Questions.Where(q => q.Id == QuestionId).First();
            question!.CorrectQuizId = itemId;
            Save?.Invoke(QuestionId, question);
        }
        private void QuizItemControl_Save(string? id, IModelBase model)
        {
            if (id == null)
            {
                QuizItems.Add(model as QuizItem);
            }

            Save?.Invoke(QuestionId, model);
        }
        private void QuizItemControl_Delete(string itemId)
        {
            var itemToRemove = QuizItems.Where(qi => qi.Id == itemId).First();
            QuizItems.Remove(itemToRemove);

            Save?.Invoke(QuestionId, _contentStore.GetQuestionById(QuestionId));
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
            if (string.IsNullOrEmpty(QuestionId))
            {
                var newQuestion = new TestingQuestion(QuestionText, QuizItems);

                Save?.Invoke(null, newQuestion);
            }
            else
            {
                var question = _contentStore.GetQuestionById(QuestionId);
                question.QuestionText = QuestionText;
                question.QuizItems = QuizItems;

                Save?.Invoke(QuestionId, question);
            }
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(QuestionId);
        }
        #endregion
    }
}
