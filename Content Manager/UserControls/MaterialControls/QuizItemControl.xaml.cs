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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Content_Manager.UserControls
{
    public partial class QuizItemControl : UserControl, IMaterialControl
    {
        public event Action<IModelBase> Create;
        public event Action<string?, IModelBase> Update;
        public event Action<string> Delete;
        public event Action<string> SetAsCorrect;

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
            btnDelete.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
        }
        private void SetUiForExistingMaterial()
        {
            btnDelete.Visibility = Visibility.Visible;
        }
        private void SharedInitialization(QuizTypes quizType, bool isExistingMaterial, bool isSelected)
        {
            InitializeComponent();
            DataContext = this;

            var propertiesToWatch = new List<string>();
            propertiesToWatch.Add(nameof(ItemText));

            // Decide what controls to make available
            _quizType = quizType;
            switch (quizType)
            {
                case QuizTypes.CelebrityWords:
                    btnChooseImage.Visibility = Visibility.Visible;

                    propertiesToWatch.Add(nameof(ItemImage));

                    break;
                case QuizTypes.Testing:
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

                    if (!isExistingMaterial)
                    {
                        btnSetAsCorrect.IsEnabled = false;
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

        public QuizItemControl(QuizTypes quizType, QuizItem quizItem, bool isSelected = false)
        {
            SharedInitialization(quizType, true, isSelected);
            SetUiForExistingMaterial();

            ItemId = quizItem.Id!;

            if (quizItem.Image != null)
            {
                ItemImage = quizItem.Image;
                OnImageSet(true);
            }

            if (quizItem.Text != null)
            {
                ItemText = quizItem.Text;
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
            string filePath = FileService.SelectImageFilePath();
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllBytes(filePath);
            if (content.Length == 0) return;

            ItemImage = content;

            OnImageSet(true);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(ItemId);
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(ItemId))
            {
                var quizItem = new QuizItem(ItemText, ItemImage);

                Create?.Invoke(quizItem);
            }
            else
            {
                var quizItem = ContentStore.GetQuizItem(ItemId);
                quizItem.Image = ItemImage;
                quizItem.Text = ItemText;

                Update?.Invoke(ItemId, quizItem);
            }
        }

        private void ValidateInput()
        {
            switch (_quizType)
            {
                case QuizTypes.GapFiller:
                    var gapOpeners = Regex.Matches(ItemText, @"\{");
                    var gapClosers = Regex.Matches(ItemText, @"\}");
                    var gappedWords = Regex.Matches(ItemText, @"\{\W*\w+.*?\}");

                    if (gapOpeners.Count != gapClosers.Count || gapOpeners.Count != gappedWords.Count)
                    {
                        throw new Exception("Неправильное форматирование");
                    }


                    if (gappedWords.Count == 0)
                    {
                        throw new Exception("Необходимо указать хотя бы одно слово для пропуска");
                    }

                    break;
            }
        }
        #endregion

        private void btnSetAsDefault_Click(object sender, RoutedEventArgs e)
        {
            if (ItemId == null)
            {
                return;
            }

            SetAsCorrect?.Invoke(ItemId);
        }
    }
}
