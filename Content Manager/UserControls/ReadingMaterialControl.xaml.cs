using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Content_Manager.UserControls {
    public partial class ReadingMaterialControl : UserControl {
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ReadingMaterial Material { get; set; }
        private const string TitleHintText = "Введите название материала";
        private int _stepsToComplete = 0;

        private bool IsModelReady() {
            return !string.IsNullOrEmpty(Material?.Content) && !string.IsNullOrEmpty(txtTitle.Text);
       
        }
        private void SetupForNewMaterial() {
            btnUploadFromFile.IsEnabled = true;

            btnPreview.Visibility = Visibility.Hidden;
            btnDelete.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
        }
        private void SetupForExistingMaterial() {
            btnUploadFromFile.Background = Brushes.LimeGreen;
            btnUploadFromFile.IsEnabled = true;

            btnPreview.IsEnabled = true;
            btnPreview.Background = Brushes.LimeGreen;

            btnDelete.Visibility = Visibility.Visible;
        }

        #region Reactions
        private void OnContentSet() {
            btnUploadFromFile.Background = Brushes.LightYellow;
        }
        #endregion

        #region Constructors
        public ReadingMaterialControl() {
            // Initialize UI
            InitializeComponent();
            DataContext = this;

            SetupForNewMaterial();

            // Setup a new UI
            Material = new ReadingMaterial() { Title = TitleHintText };
        }

        public ReadingMaterialControl(ReadingMaterial material) {
            // Initialize UI
            InitializeComponent();
            DataContext = this;

            SetupForExistingMaterial();

            Material = material;
        }
        #endregion

        #region TitleHandlers
        private void txtTitle_GotFocus(object sender, RoutedEventArgs e) {
            // Clear the hint text when clicking on title field
            if (Material.Title == TitleHintText && string.IsNullOrEmpty(Material.Content)) {
                Material.Title = "";
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e) {
            // Set the hint text when losing focust from the title field, when nothing has been written
            if (string.IsNullOrEmpty(Material.Title) && string.IsNullOrEmpty(Material.Content)) {
                Material.Title = TitleHintText;
            }
        }
        #endregion

        private void btnSave_Click(object sender, RoutedEventArgs e) {
            // TODO: Change so that Add automatically saves and triggers refresh
            if (!IsModelReady()) {
                MessageBox.Show("Укажите все необходимые данные для материала");
                return;
            }

            ContentStore.SelectedSegment?.ReadingMaterials.Add(Material);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
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

            OnContentSet();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e) {
            ContentStore.SelectedSegment!.ReadingMaterials.Remove(Material);
            ContentStore.UpdateItem<Segment>(ContentStore.SelectedSegment);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
    }
}
