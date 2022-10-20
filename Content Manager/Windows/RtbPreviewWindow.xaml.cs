using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Content_Manager.Windows {
    public partial class RtbPreviewWindow : Window {
        public RtbPreviewWindow(string title, string content) {
            InitializeComponent();
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(content));

            TextRange textRange = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
            textRange.Load(stream, DataFormats.Xaml);
        }
    }
}
