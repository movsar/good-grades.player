using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Content_Manager.Windows
{
    public partial class RtbPreviewWindow : Window
    {
        private void LoadDocument(string rtf, byte[] image)
        {
            if (image != null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.StreamSource = new MemoryStream(image);
                logo.EndInit();

                imgMain.Source = logo;
            }

            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
            var txtRange = new TextRange(flowParagraph.ContentStart, flowParagraph.ContentEnd);
            txtRange.Load(stream, DataFormats.Rtf);
        }
        public RtbPreviewWindow(string title, string content, byte[] image)
        {
            InitializeComponent();
            LoadDocument(content, image);
        }
    }
}
