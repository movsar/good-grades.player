using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;

namespace Content_Manager.Windows {
    public partial class RtbPreviewWindow : Window {
        private void docxToFlow()
        {
            Package package = Package.Open(@"C:\Users\x.dr\Desktop\Aishat\Good Grades\TestMaterials\StoryWithPicture.docx");
            Uri documentUri = new Uri("/word/document.xml", UriKind.Relative);
            PackagePart documentPart = package.GetPart(documentUri);
            XElement wordDoc = XElement.Load(new StreamReader(documentPart.GetStream()));

            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            var paragraphs = from p in wordDoc.Descendants(w + "p")
                             select p;

            foreach (var p in paragraphs)
            {
                var style = from s in p.Descendants(w + "pPr")
                            select s;

                var font = (from f in style.Descendants(w + "rFonts")
                            select f.FirstAttribute).FirstOrDefault();
                var size = (from s in style.Descendants(w + "sz")
                            select s.FirstAttribute).FirstOrDefault();

                var pics = (from s in style.Descendants(w + "nvPicPr")
                            select s.FirstAttribute).FirstOrDefault();

                Paragraph par = new Paragraph();
                Run r = new Run(p.Value);
                if (font != null)
                {
                    FontFamilyConverter converter = new FontFamilyConverter();
                    r.FontFamily = (FontFamily)converter.ConvertFrom(font.Value);
                }
                if (size != null)
                {
                    r.FontSize = double.Parse(size.Value);
                }
                par.Inlines.Add(r);

                //flowDocument.Blocks.Add(par);
            }

        }
        public RtbPreviewWindow(string title, string content) {
            InitializeComponent();
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(content));
            TextRange textRange = new TextRange(rtbMain.Document.ContentStart, rtbMain.Document.ContentEnd);
            textRange.Load(stream, DataFormats.Rtf);
        }
    }
}
