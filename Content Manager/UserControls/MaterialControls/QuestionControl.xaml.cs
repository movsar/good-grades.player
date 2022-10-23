using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Data.Enums;
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

        private void RefreshUI()
        {
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
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

            var propertiesToWatch = new List<string>() { nameof(QuestionText) };

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

            foreach (var quizItem in testingQuestion.QuizItems)
            {
                var quizItemControl = new QuizItemControl(QuizTypes.Testing, quizItem);
                spItems.Children.Add(quizItemControl);
            }
            spItems.Children.Add(new QuizItemControl(QuizTypes.Testing));


            OnTextSet(true);
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

        private void btnSaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(QuestionId))
            {
                var newOption = new TestingQuestion(QuestionText, QuizItems);
                ContentStore.AddQuestion(newOption);
            }
            else
            {
                var question = ContentStore.GetQuestionById(QuestionId);
                question.QuestionText = QuestionText;
                question.QuizItems = QuizItems;

                ContentStore.UpdateTestingQuiz();
            }

            RefreshUI();
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            ContentStore.DeleteQuestion(QuestionId);
            RefreshUI();
        }

        //private void _contentStore_SegmentChanged(Segment selectedSegment)
        //{
        //    if (selectedSegment == null) return;

        //    spItems.Children.Clear();

        //    foreach (var question in selectedSegment.TestingQuiz.Questions)
        //    {
        //        var questionControl = new QuestionControl(question);
        //        spItems.Children.Add(questionControl);
        //    }

        //    spItems.Children.Add(new QuestionControl());
        //}
    }
}
