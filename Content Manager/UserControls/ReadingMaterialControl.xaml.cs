using Content_Manager.Stores;
using Content_Manager.Windows;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ReadingMaterialControl.xaml
    /// </summary>
    public partial class ReadingMaterialControl : UserControl
    {
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public ReadingMaterial Material { get; set; }
        private void Init()
        {
            InitializeComponent();
            DataContext = this;
            btnEdit.IsEnabled = false;
        }
        public ReadingMaterialControl()
        {
            Init();
            Material = new ReadingMaterial()
            {
                Title = "Введите название материала"
            };
        }
        public ReadingMaterialControl(ReadingMaterial material)
        {
            Init();
            Material = material;
            toggleIsEnabled();
        }

        private void txtTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Material.Title == "Введите название материала")
            {
                Material.Title = "";
            }
        }

        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Material.Title == "")
            {
                Material.Title = "Введите название материала";
            }
        }

        private void toggleIsEnabled()
        {
            btnPreview.IsEnabled = !btnPreview.IsEnabled;
            btnUploadFromFile.IsEnabled = !btnUploadFromFile.IsEnabled;
            txtTitle.IsEnabled = !txtTitle.IsEnabled;
            btnEdit.IsEnabled = !btnEdit.IsEnabled;
        }

        private void btnEditClick(object sender, RoutedEventArgs e)
        {
            toggleIsEnabled();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            var rtbPreviewWindow = new RtbPreviewWindow(Material);
            rtbPreviewWindow.ShowDialog();
        }

        private void btnUploadFromFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "RTF документы (.rtf)|*.rtf"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == false)
            {
                return;
            }

            string filename = dialog.FileName;
            var contents = File.ReadAllText(filename);

            Material.Content = contents;
            Material.Title = txtTitle.Text;
            ContentStore.SelectedSegment?.ReadingMaterials.Add(Material);
        }
    }
}
