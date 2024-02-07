using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

namespace Shared.Services
{
    public static class RtfService
    {
        public static void LoadRtfFromText(RichTextBox richTextBox, string rtfString)
        {
            var range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rtfString ?? "")))
            {
                range.Load(stream, DataFormats.Rtf);
            }
        }
        public static string GetRtfDescriptionAsText(RichTextBox richTextBox)
        {
            var range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            using (var stream = new MemoryStream())
            {
                range.Save(stream, DataFormats.Rtf);
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
