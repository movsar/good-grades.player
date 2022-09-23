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
using System.Windows.Shapes;

namespace Content_Manager.Windows
{
    /// <summary>
    /// Interaction logic for RtbPreviewWindow.xaml
    /// </summary>
    public partial class RtbPreviewWindow : Window
    {
        public RtbPreviewWindow(string filePath)
        {
            InitializeComponent();
            LoadFile(filePath);
        }
        private void LoadFile(string filePath)
        {
            var contentts = File.ReadAllText(filePath);
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(contentts));

            rtbMain.Selection.Load(stream, DataFormats.Rtf);
            rtbMain.Selection.Select(rtbMain.Document.ContentEnd, rtbMain.Document.ContentEnd);
        }
    }
}
