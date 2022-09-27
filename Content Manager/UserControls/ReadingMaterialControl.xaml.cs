using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Content_Manager.UserControls
{
    public partial class ReadingMaterialControl : UserControl
    {
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ReadingMaterial Material { get; set; }
        private const string TitleHintText = "Введите название материала";
        private void Init()
        {
            InitializeComponent();
            DataContext = this;
        }
        public ReadingMaterialControl()
        {
            Init();
            Material = new ReadingMaterial()
            {
                Title = TitleHintText
            };
            btnDelete.Visibility = Visibility.Hidden;
        }
        public ReadingMaterialControl(ReadingMaterial material)
        {
            Init();
            Material = material;
            btnPreview.IsEnabled = true;
            btnPreview.Background = Brushes.LimeGreen;
            btnDelete.Visibility = Visibility.Visible;
        }

        private void txtTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Material.Title == TitleHintText && string.IsNullOrEmpty(Material.Content))
            {
                Material.Title = "";
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Material.Title) && string.IsNullOrEmpty(Material.Content))
            {
                Material.Title = TitleHintText;
            }
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            var rtbPreviewWindow = new RtbPreviewWindow(Material);
            rtbPreviewWindow.ShowDialog();
        }


        private void btnUploadFromFile_Click(object sender, RoutedEventArgs e)
        {
            // Read the rtf file
            string filePath = FileService.SelectFilePath("RTF документы (.rtf)|*.rtf");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var contents = File.ReadAllText(filePath);
            Material.Content = contents;
            ContentStore.SelectedSegment?.ReadingMaterials.Add(Material);

            // Refresh UI
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTitle.Text == TitleHintText || string.IsNullOrEmpty(txtTitle.Text))
            {
                btnUploadFromFile.IsEnabled = false;
            }
            else
            {
                btnUploadFromFile.IsEnabled = true;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ContentStore.SelectedSegment!.ReadingMaterials.Remove(Material);
            ContentStore.UpdateItem<Segment>(ContentStore.SelectedSegment);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
    }
}
