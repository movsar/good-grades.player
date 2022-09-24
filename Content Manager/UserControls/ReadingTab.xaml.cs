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
using System.IO;
using Content_Manager.Windows;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Content_Manager.Stores;

namespace Content_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for ReadingTab.xaml
    /// </summary>
    public partial class ReadingTab : UserControl
    {
        private ContentStore _contentStore { get; }
        public ReadingTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
           
        }

        private void _contentStore_SegmentChanged(Segment selectedSegment)
        {
            spReadingMaterialControls.Children.Clear();

            foreach (var material in selectedSegment.ReadingMaterials)
            {
                spReadingMaterialControls.Children.Add(new ReadingMaterialControl(_contentStore, material));
            }

            spReadingMaterialControls.Children.Add(new ReadingMaterialControl(_contentStore));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rtbPreviewWindow = new RtbPreviewWindow(@"C:\users\x.dr\desktop\aaa.rtf");
            rtbPreviewWindow.ShowDialog();
        }
    }
}
