using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

namespace Content_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for ListeningMaterialControl.xaml
    /// </summary>
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
        public ListeningMaterialControl(ListeningMaterial material)
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
            //var rtbPreviewWindow = new RtbPreviewWindow(Material);
            //rtbPreviewWindow.ShowDialog();
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btnChooseAudio_Click(object sender, RoutedEventArgs e)
        {
        }

        //private void btnUploadFromFile_Click(object sender, RoutedEventArgs e)
        //{
        //    // Read the rtf file
        //    string filePath = FileService.SelectFilePath();
        //    if (string.IsNullOrEmpty(filePath)) return;

        //    // Read, load contents to the object and add to collection
        //    var contents = File.ReadAllText(filePath);
        //    Material.Content = contents;
        //    ContentStore.SelectedSegment?.ReadingMaterials.Add(Material);

        //    // Refresh UI
        //    ContentStore.SelectedSegment = ContentStore.SelectedSegment;
        //}

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTitle.Text == TitleHintText || string.IsNullOrEmpty(txtTitle.Text))
            {
                btnChooseAudio.IsEnabled = false;
                btnChooseImage.IsEnabled = false;
            }
            else
            {
                btnChooseAudio.IsEnabled = true;
                btnChooseImage.IsEnabled = true;
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
