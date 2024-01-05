using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Shared.Viewers
{
    public partial class ReadingViewer : Window
    {

        #region Initialization
        private void SharedInitialization()
        {
            InitializeComponent();
        }
        public ReadingViewer(string title, string text, byte[] image)
        {
            // Reading material presenter mode

            Title = title;

            SharedInitialization();
            LoadDocument(text, image);
        }
        #endregion

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
            else
            {
                flowDocument.Blocks.Remove(flowImageParagraph);
            }

            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
            var txtRange = new TextRange(flowContentParagraph.ContentStart, flowContentParagraph.ContentEnd);
            txtRange.Load(stream, DataFormats.Rtf);
        }
    }
}
