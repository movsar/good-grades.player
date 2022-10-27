using Content_Manager.Interfaces;
using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Controls;
using Shared.Viewers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls
{
    public partial class ReadingMaterialControl : UserControl, IMaterialControl
    {
        public event Action<string?, IModelBase> Save;
        public event Action<string> Delete;

        #region Fields
        private FormCompletionInfo _formCompletionInfo;
        private const string TitleHintText = "Введите название материала";
        #endregion

        #region Properties
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();

        public string RmTitle
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("RmTitle", typeof(string), typeof(ReadingMaterialControl), new PropertyMetadata(""));

        public string RmText { get; set; }
        public byte[] RmImage { get; set; }
        private string RmId { get; }

        #endregion

        #region Reactions
        private void OnFormStatusChanged(bool isReady)
        {
            if (isReady)
            {
                btnSave.Visibility = Visibility.Visible;
                btnPreview.Visibility = Visibility.Visible;
            }
            else
            {
                btnSave.Visibility = Visibility.Collapsed;
                btnPreview.Visibility = Visibility.Collapsed;
            }
        }
        private void OnTitleSet(bool isSet)
        {
            _formCompletionInfo.Update(nameof(RmTitle), isSet);
        }
        private void OnContentSet(bool isSet = true)
        {
            btnUploadFromFile.Background = StylingService.StagedBrush;

            _formCompletionInfo.Update(nameof(RmText), isSet);
        }
        private void OnImageSet(bool isSet = true)
        {
            btnChooseImage.Background = StylingService.StagedBrush;

            _formCompletionInfo.Update(nameof(RmImage), isSet);
        }
        #endregion

        #region Initialization
        private void SetUiForNewMaterial()
        {
            btnPreview.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
        }
        private void SetUiForExistingMaterial()
        {
            btnUploadFromFile.Background = StylingService.ReadyBrush;

            if (RmImage != null)
            {
                btnChooseImage.Background = StylingService.ReadyBrush;
            }

            btnPreview.Background = StylingService.ReadyBrush;
            btnDelete.Visibility = Visibility.Visible;
        }

        private void SharedInitialization(bool isExistingMaterial = false)
        {
            InitializeComponent();
            DataContext = this;

            var propertiesToWatch = new List<string>();
            propertiesToWatch.Add(nameof(RmTitle));
            propertiesToWatch.Add(nameof(RmText));

            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingMaterial);
            _formCompletionInfo.StatusChanged += OnFormStatusChanged;
        }

        public ReadingMaterialControl()
        {
            SharedInitialization();
            SetUiForNewMaterial();

            RmTitle = TitleHintText;
        }

        public ReadingMaterialControl(ReadingMaterial material)
        {
            SharedInitialization(true);

            RmId = material.Id!;
            RmTitle = material.Title;
            RmText = material.Text;
            RmImage = material.Image;

            SetUiForExistingMaterial();
        }
        #endregion

        #region TitleHandlers
        private void txtTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RmTitle == TitleHintText)
            {
                RmTitle = "";
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(RmTitle))
            {
                RmTitle = TitleHintText;
            }
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || txtTitle.Text.Equals(TitleHintText))
            {
                OnTitleSet(false);
            }
            else
            {
                OnTitleSet(true);
            }
        }
        #endregion

        #region ButtonHandlers
        private void btnUploadFromFile_Click(object sender, RoutedEventArgs e)
        {
            // Read the rtf file
            string filePath = FileService.OpenFilePath("Файлы с RTF текстом (.rtf) | *.rtf;");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var contents = File.ReadAllText(filePath);
            RmText = contents;

            OnContentSet(true);
        }
        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            var rtbPreviewWindow = new MaterialPresenter(RmTitle, RmText, RmImage);
            rtbPreviewWindow.ShowDialog();
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.OpenFilePath("Файлы изображений (.png) | *.png; *.jpg; *.jpeg; *.tiff");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllBytes(filePath);
            if (content.Length == 0) return;

            RmImage = content;
            OnImageSet(true);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Укажите все необходимые данные для материала");

            if (string.IsNullOrEmpty(RmId))
            {
                var newRm = new ReadingMaterial(RmTitle, RmText, RmImage);
                ContentStore.SelectedSegment?.ReadingMaterials.Add(newRm);

                Save?.Invoke(null, newRm);
            }
            else
            {
                var rm = ContentStore.GetReadingMaterialById(RmId);
                rm.Title = RmTitle;
                rm.Text = RmText;
                rm.Image = RmImage;

                Save?.Invoke(RmId, rm);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(RmId);
        }
        #endregion

    }
}
