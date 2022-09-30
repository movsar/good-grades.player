using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Content_Manager.UserControls
{
    public partial class ListeningMaterialControl : UserControl
    {

        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ListeningMaterial Material { get; set; }
        private const string TitleHintText = "Введите название материала";
        private void Init()
        {
            InitializeComponent();
            DataContext = this;
        }
        public ListeningMaterialControl()
        {
            Init();
            Material = new ListeningMaterial()
            {
                Title = TitleHintText
            };
            btnDelete.Visibility = Visibility.Hidden;
        }
        private void SaveIfReady()
        {
            if (Material.Audio != null && Material.Image != null && Material.Content != null)
            {
                if (Material.Id == null)
                {
                    ContentStore.SelectedSegment?.ListeningMaterials.Add(Material);
                }

                ContentStore.UpdateItem<Segment>(ContentStore.SelectedSegment!);
                Task.Delay(5000);
                ContentStore.SelectedSegment = ContentStore.SelectedSegment;
            }
        }
        private static void SetButtonStyle(Button button, string modelId, bool isContentSet)
        {
            if (!isContentSet) return;
            if (modelId == null)
            {
                button.Background = Brushes.LightYellow;
            }
            else
            {
                button.Background = Brushes.LightGreen;
            }
        }

        public ListeningMaterialControl(ListeningMaterial material)
        {
            Init();
            Material = material;
            if (material.Id != null && material.Audio != null && material.Image != null && material.Content != null)
            {
                btnPreview.IsEnabled = true;
                btnPreview.Background = Brushes.LightGreen;
                btnDelete.Visibility = Visibility.Visible;
                btnDelete.Background = Brushes.Red;
            }

            SetButtonStyle(btnChooseAudio, material.Id, material.Audio != null);
            SetButtonStyle(btnChooseImage, material.Id, material.Image != null);
            SetButtonStyle(btnChooseText, material.Id, material.Content != null);
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
            var listeningPreviewWindow = new ListeningPreviewWindow(Material);
            listeningPreviewWindow.ShowDialog();
        }

        private void btnChooseText_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectFilePath("Файлы с текстом (.txt) | *.txt;");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(filePath)) return;

            Material.Content = content;
            SetButtonStyle(btnChooseText, Material.Id, true);

            SaveIfReady();
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectFilePath("Файлы изображений (.png) | *.png; *.jpg; *.jpeg; *.tiff");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllBytes(filePath);
            if (content.Length == 0) return;

            Material.Image = content;
            SetButtonStyle(btnChooseImage, Material.Id, true);

            SaveIfReady();

            // Refresh UI
            // ContentStore.SelectedSegment = ContentStore.SelectedSegment;

        }
        private void btnChooseAudio_Click(object sender, RoutedEventArgs e)
        {
            // Read the rtf file
            string filePath = FileService.SelectFilePath("MP3 Файлы (.mp3) | *.mp3");
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllBytes(filePath);
            if (content.Length == 0) return;

            Material.Audio = content;
            SetButtonStyle(btnChooseAudio, Material.Id, true);

            SaveIfReady();
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTitle.Text == TitleHintText || string.IsNullOrEmpty(txtTitle.Text))
            {
                btnChooseAudio.IsEnabled = false;
                btnChooseImage.IsEnabled = false;
                btnChooseText.IsEnabled = false;
            }
            else
            {
                btnChooseAudio.IsEnabled = true;
                btnChooseImage.IsEnabled = true;
                btnChooseText.IsEnabled = true;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ContentStore.SelectedSegment!.ListeningMaterials.Remove(Material);
            ContentStore.UpdateItem<Segment>(ContentStore.SelectedSegment);
            ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        }
    }
}
