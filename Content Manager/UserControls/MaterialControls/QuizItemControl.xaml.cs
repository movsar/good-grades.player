using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Content_Manager.UserControls
{
    public partial class QuizItemControl : UserControl
    {

        #region Fields
        private const string Hint = "Введите описание";
        private FormCompletionInfo _formCompletionInfo;
        private QuizTypes _quizType;
        #endregion

        #region Properties
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();
        public string ItemText
        {
            get { return (string)GetValue(ItemTextProperty); }
            set { SetValue(ItemTextProperty, value); }
        }
        public static readonly DependencyProperty ItemTextProperty =
            DependencyProperty.Register("ItemText", typeof(string), typeof(QuizItemControl), new PropertyMetadata(""));

        public string ItemId { get; }
        private byte[] ItemImage { get; set; }

        #endregion

        #region Reactions

        private void OnFormStatusChanged(bool isReady)
        {
            if (isReady)
            {
                btnSave.Visibility = Visibility.Visible;
            }
            else
            {
                btnSave.Visibility = Visibility.Collapsed;
            }
        }
        private void OnTextSet(bool isSet)
        {
            _formCompletionInfo.Update(nameof(ItemText), isSet);
        }
        private void OnImageSet(bool isSet = true)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.StreamSource = new MemoryStream(ItemImage);
            logo.EndInit();

            var imgControl = new Image();
            imgControl.VerticalAlignment = VerticalAlignment.Stretch;
            imgControl.Source = logo;
            btnChooseImage.Content = imgControl;

            _formCompletionInfo.Update(nameof(ItemImage), isSet);
        }

        #endregion

        #region Initialization
        private void SetUiForNewMaterial()
        {
            btnDelete.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
        }
        private void SetUiForExistingMaterial()
        {
            btnDelete.Visibility = Visibility.Visible;
        }
        private void SharedInitialization(QuizTypes quizType, bool isExistingMaterial, bool isSelected)
        {
            InitializeComponent();
            DataContext = this;

            _quizType = quizType;
            var propertiesToWatch = new List<string>() { nameof(ItemText) };

            switch (quizType)
            {
                case QuizTypes.CelebrityWords:
                    btnChooseImage.Visibility = Visibility.Visible;

                    propertiesToWatch.Add(nameof(ItemImage));
                    break;
                case QuizTypes.ProverbSelection:
                    btnSetAsCorrect.Visibility = Visibility.Visible;

                    if (isSelected)
                    {
                        btnSetAsCorrect.Content = "\uE73A";
                        btnSetAsCorrect.Foreground = Brushes.DarkGreen;
                    }
                    else
                    {
                        btnSetAsCorrect.Content = "\uE739";
                    }

                    break;
                default:
                    break;
            }

            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingMaterial);
            _formCompletionInfo.StatusChanged += OnFormStatusChanged;
        }
        public QuizItemControl(QuizTypes quizType)
        {
            SharedInitialization(quizType, false, false);
            SetUiForNewMaterial();

            ItemText = Hint;
        }

        public QuizItemControl(QuizTypes quizType, string itemId, byte[] image, string text, bool isSelected = false)
        {
            // Constructor for celebrity words quiz

            SharedInitialization(quizType, true, isSelected);
            SetUiForExistingMaterial();

            ItemId = itemId;

            if (image != null)
            {
                ItemImage = image;
                OnImageSet(true);
            }

            if (text != null)
            {
                ItemText = text;
                OnTextSet(true);
            }
        }
        #endregion

        #region TextHandlers
        private void txtItemText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ItemText == Hint)
            {
                ItemText = "";
            }
        }

        private void txtItemText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ItemText))
            {
                ItemText = Hint;
            }
        }

        private void txtItemText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemText.Text) || txtItemText.Text.Equals(Hint))
            {
                OnTextSet(false);
            }
            else
            {
                OnTextSet(true);
            }
        }
        #endregion

        #region ButtonHandlers
        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectFilePath("Файлы изображений (.png) | *.png; *.jpg; *.jpeg; *.tiff");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllBytes(filePath);
            if (content.Length == 0) return;

            ItemImage = content;

            OnImageSet(true);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ContentStore.DeleteQuizItem(_quizType, ItemId);

            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ItemId))
            {
                var newOption = new QuizItem(ItemImage, ItemText);
                ContentStore.AddQuizItem(_quizType, newOption);
            }
            else
            {
                var quizItem = ContentStore.GetQuizItem(ItemId);
                quizItem.Image = ItemImage;
                quizItem.Text = ItemText;

                ContentStore.UpdateQuiz(_quizType);
            }

            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
        #endregion

        private void btnSetAsDefault_Click(object sender, RoutedEventArgs e)
        {
            if (ItemId == null)
            {
                return;
            }

            if (_quizType == QuizTypes.ProverbSelection)
            {
                ContentStore!.SelectedSegment!.ProverbSelectionQuiz!.CorrectProverbId = ItemId;
                ContentStore.UpdateQuiz(_quizType);
             
                ContentStore.SelectedSegment = ContentStore.SelectedSegment;
            }
        }
    }
}
