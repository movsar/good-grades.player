﻿using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls {
    public partial class ReadingMaterialControl : UserControl {

        #region Fields
        private FormCompletionInfo _formCompletionInfo;
        private const string TitleHintText = "Введите название материала";
        #endregion

        #region Properties
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();

        public string RmTitle {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("RmTitle", typeof(string), typeof(ReadingMaterialControl), new PropertyMetadata(""));

        public string RmText { get; set; }
        private string RmId { get; }

        #endregion

        #region Reactions
        private void OnFormStatusChanged(bool isReady) {
            if (isReady) {
                btnSave.Visibility = Visibility.Visible;
                btnPreview.Visibility = Visibility.Visible;
            } else {
                btnSave.Visibility = Visibility.Collapsed;
                btnPreview.Visibility = Visibility.Collapsed;
            }
        }
        private void OnTitleSet(bool isSet) {
            _formCompletionInfo.Update(nameof(RmTitle), isSet);
        }
        private void OnContentSet(bool isSet = true) {
            btnUploadFromFile.Background = StylingService.StagedBrush;

            _formCompletionInfo.Update(nameof(RmText), isSet);
        }
        #endregion

        #region Initialization
        private void SetUiForNewMaterial() {
            btnPreview.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;

            btnPreview.Background = StylingService.StagedBrush;
        }
        private void SetUiForExistingMaterial() {
            btnUploadFromFile.Background = StylingService.ReadyBrush;
            btnPreview.Background = StylingService.ReadyBrush;
            btnDelete.Visibility = Visibility.Visible;
        }

        private void SharedInitialization(bool isExistingMaterial = false) {
            InitializeComponent();
            DataContext = this;

            var propertiesToWatch = new string[] { nameof(RmTitle), nameof(RmText) };
            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingMaterial);
            _formCompletionInfo.StatusChanged += OnFormStatusChanged;
        }
        public ReadingMaterialControl() {
            SharedInitialization();
            SetUiForNewMaterial();

            RmTitle = TitleHintText;
        }

        public ReadingMaterialControl(ReadingMaterial material) {
            SharedInitialization(true);
            SetUiForExistingMaterial();

            RmId = material.Id!;
            RmTitle = material.Title;
            RmText = material.Text;
        }
        #endregion

        #region TitleHandlers
        private void txtTitle_GotFocus(object sender, RoutedEventArgs e) {
            if (RmTitle == TitleHintText) {
                RmTitle = "";
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(RmTitle)) {
                RmTitle = TitleHintText;
            }
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e) {
            if (string.IsNullOrEmpty(txtTitle.Text) || txtTitle.Text.Equals(TitleHintText)) {
                OnTitleSet(false);
            } else {
                OnTitleSet(true);
            }
        }
        #endregion

        #region ButtonHandlers
        private void btnUploadFromFile_Click(object sender, RoutedEventArgs e) {
            // Read the rtf file
            string filePath = FileService.SelectFilePath("RTF документы (.rtf)|*.rtf");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var contents = File.ReadAllText(filePath);
            RmText = contents;

            OnContentSet(true);
        }
        private void btnPreview_Click(object sender, RoutedEventArgs e) {
            var rtbPreviewWindow = new RtbPreviewWindow(RmTitle, RmText);
            rtbPreviewWindow.ShowDialog();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            //MessageBox.Show("Укажите все необходимые данные для материала");

            if (string.IsNullOrEmpty(RmId)) {
                ContentStore.SelectedSegment?.ReadingMaterials
                    .Add(new ReadingMaterial(RmTitle, RmText));
            } else {
                var rm = ContentStore.GetReadingMaterialById(RmId);
                rm.Title = RmTitle;
                rm.Text = RmText;
            }

            ContentStore.UpdateSegment(ContentStore!.SelectedSegment!);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            ContentStore.SelectedSegment!.ReadingMaterials.Remove(ContentStore.GetReadingMaterialById(RmId));
            ContentStore.UpdateSegment(ContentStore.SelectedSegment);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
        #endregion

    }
}