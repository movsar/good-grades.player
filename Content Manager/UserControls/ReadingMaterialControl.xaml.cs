using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Content_Manager.UserControls {
    public partial class ReadingMaterialControl : UserControl {
        enum FormValues {
            Title,
            Content
        }
        class FormCompletionInfo {
            public event Action<bool> StatusChanged;
            public bool IsReady => _stats.Where(s => s.Value == true).Count() == _stats.Count();
            private readonly StylingService _stylingService;

            private readonly Dictionary<string, bool> _stats = new Dictionary<string, bool>();

            public FormCompletionInfo(bool existingElement) {
                // Initialize the dictionary
                foreach (var v in Enum.GetNames<FormValues>()) {
                    _stats.Add(v, existingElement);
                }
            }

            public void Update(FormValues formValue, bool isSet) {
                if (_stats[Enum.GetName(formValue)] == isSet) {
                    return;
                }

                _stats[Enum.GetName(formValue)] = isSet;

                StatusChanged?.Invoke(IsReady);
            }
        }

        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();
        public ReadingMaterial Material { get; set; }
        private const string TitleHintText = "Введите название материала";
        private readonly FormCompletionInfo formCompletionInfo;



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
            formCompletionInfo.Update(FormValues.Title, isSet);
        }
        private void OnContentSet(bool isSet = true) {
            btnUploadFromFile.Background = StylingService.StagedBrush;

            formCompletionInfo.Update(FormValues.Content, isSet);
        }
        #endregion

        #region Initialization
        private void SetUiForNewMaterial() {
            DataContext = this;

            btnPreview.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
        }
        private void SetUiForExistingMaterial() {
            DataContext = this;

            btnUploadFromFile.Background = StylingService.ReadyBrush;
            btnPreview.Background = StylingService.ReadyBrush;
            btnDelete.Visibility = Visibility.Visible;
        }

        public ReadingMaterialControl() {
            InitializeComponent();
            SetUiForNewMaterial();

            formCompletionInfo = new FormCompletionInfo(false);
            formCompletionInfo.StatusChanged += OnFormStatusChanged;

            Material = new ReadingMaterial() { Title = TitleHintText };
        }

        public ReadingMaterialControl(ReadingMaterial material) {
            InitializeComponent();
            SetUiForExistingMaterial();

            formCompletionInfo = new FormCompletionInfo(true);
            formCompletionInfo.StatusChanged += OnFormStatusChanged;

            Material = material;
        }
        #endregion

        #region TitleHandlers
        private void txtTitle_GotFocus(object sender, RoutedEventArgs e) {
            if (Material.Title == TitleHintText) {
                Material.Title = "";
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(Material.Title)) {
                Material.Title = TitleHintText;
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

        #region Buttons
        private void btnSave_Click(object sender, RoutedEventArgs e) {
            //MessageBox.Show("Укажите все необходимые данные для материала");

            if (string.IsNullOrEmpty(Material.Id)) {
                ContentStore.SelectedSegment?.ReadingMaterials.Add(Material);
            }

            ContentStore.UpdateItem<Segment>(ContentStore!.SelectedSegment!);
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e) {
            var rtbPreviewWindow = new RtbPreviewWindow(Material);
            rtbPreviewWindow.ShowDialog();
        }

        private void btnUploadFromFile_Click(object sender, RoutedEventArgs e) {
            // Read the rtf file
            string filePath = FileService.SelectFilePath("RTF документы (.rtf)|*.rtf");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var contents = File.ReadAllText(filePath);
            Material.Content = contents;

            OnContentSet(true);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            ContentStore.SelectedSegment!.ReadingMaterials.Remove(Material);
            ContentStore.UpdateItem<Segment>(ContentStore.SelectedSegment);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
        #endregion

    }
}
