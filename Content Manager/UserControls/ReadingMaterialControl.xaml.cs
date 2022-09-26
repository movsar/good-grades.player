using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

        private static string SelectFilePath()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "RTF документы (.rtf)|*.rtf"; 

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            return (result == false) ? "" : dialog.FileName;
        }
        private void btnUploadFromFile_Click(object sender, RoutedEventArgs e)
        {
            // Read the rtf file
            string filePath = SelectFilePath();
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            var contents = File.ReadAllText(filePath);

            // Load contents to the object and add to collection
            Material.Content = contents;
            ContentStore.SelectedSegment?.ReadingMaterials.Add(Material);
            
            // Refresh UI
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
            btnPreview.IsEnabled = true;
            btnDelete.Visibility = Visibility.Visible;
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTitle.Text == TitleHintText || string.IsNullOrEmpty(txtTitle.Text))
            {
                btnUploadFromFile.IsEnabled = false;
            } else
            {
                btnUploadFromFile.IsEnabled = true;
            }
        }

        private void DeleteSegment(Segment segment)
        {
            segment!.ReadingMaterials.Remove(Material);
            ContentStore.UpdateItem<Segment>(segment);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSegment(ContentStore.SelectedSegment!);
        }
    }
}
