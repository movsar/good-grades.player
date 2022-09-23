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

namespace Content_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for ReadingTab.xaml
    /// </summary>
    public partial class ReadingTab : UserControl
    {
        public Segment SelectedSegment
        {
            get => (Segment)GetValue(SelectedSegmentProperty);
            set => SetValue(SelectedSegmentProperty, value);
        }


        public ReadingTab()
        {
            InitializeComponent();
            DataContext = this;

            spReadingMaterialControls.Children.Add(new ReadingMaterialControl());
            spReadingMaterialControls.Children.Add(new ReadingMaterialControl());
            spReadingMaterialControls.Children.Add(new ReadingMaterialControl());
            spReadingMaterialControls.Children.Add(new ReadingMaterialControl());
            spReadingMaterialControls.Children.Add(new ReadingMaterialControl());
            spReadingMaterialControls.Children.Add(new ReadingMaterialControl());
        }

      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rtbPreviewWindow = new RtbPreviewWindow(@"C:\users\x.dr\desktop\aaa.rtf");
            rtbPreviewWindow.ShowDialog();
        }
    }
}
