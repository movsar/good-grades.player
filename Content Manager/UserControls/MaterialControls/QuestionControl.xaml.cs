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
    public partial class QuestionControl : UserControl
    {
        #region Events
        public event Action<TestingQuestion> Add;
        public event Action Save;
        public event Action<string> Delete;
        #endregion

        #region Fields
        private const string Hint = "Введите описание";
        private FormCompletionInfo _formCompletionInfo;
        private QuizTypes _quizType;
        #endregion

        #region Properties
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
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
            btnDeleteQuestion.Visibility = Visibility.Collapsed;
            btnSaveQuestion.Visibility = Visibility.Collapsed;
        }
        private void SetUiForExistingMaterial()
        {
            btnDeleteQuestion.Visibility = Visibility.Visible;
        }
        private void SharedInitialization(bool isExistingMaterial)
        {
            InitializeComponent();
            DataContext = this;

            var propertiesToWatch = new Dictionary<string, object>();
            propertiesToWatch.Add(nameof(QuestionText), QuestionText);

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
                var quizItemControl = new QuizItemControl(QuizTypes.Testing, quizItem);
                quizItemControl.Add += QuizItemControl_Add;
                quizItemControl.Save += QuizItemControl_Save;
                quizItemControl.Delete += QuizItemControl_Delete;
                spItems.Children.Add(quizItemControl);
            }

            var newQuizItemControl = new QuizItemControl(QuizTypes.Testing);
            newQuizItemControl.Add += QuizItemControl_Add;
            newQuizItemControl.Save += QuizItemControl_Save;
            newQuizItemControl.Delete += QuizItemControl_Delete;

            spItems.Children.Add(newQuizItemControl);

            OnTextSet(true);
        }
        #endregion

        #region Event Handlers
        private void QuizItemControl_Save()
        {
            Save?.Invoke();
        }
        private void QuizItemControl_Add(QuizItem quizItem)
        {
            QuizItems.Add(quizItem);
            Save?.Invoke();
        }
        private void QuizItemControl_Delete(string itemId)
        {
            var itemToRemove = QuizItems.Where(qi => qi.Id == itemId).First();
            QuizItems.Remove(itemToRemove);
            Save?.Invoke();
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

                Add?.Invoke(newQuestion);
            }
            else
            {
                var question = ContentStore.GetQuestionById(QuestionId);
                question.QuestionText = QuestionText;
                question.QuizItems = QuizItems;

                Save?.Invoke();
            }
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(QuestionId);
        }
        #endregion
    }
}
