using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Content_Manager.Windows
{
    public partial class RtbPreviewWindow : Window
    {
        public RtbPreviewWindow(string title, string content)
        {
            InitializeComponent();
            MemoryStream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(content));
            var txtRange = new TextRange(flowParagraph.ContentStart, flowParagraph.ContentEnd);
            txtRange.Load(stream, DataFormats.Rtf);
        }
    }
}
