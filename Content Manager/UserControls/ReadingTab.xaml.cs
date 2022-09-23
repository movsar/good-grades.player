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

namespace Content_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for ReadingTab.xaml
    /// </summary>
    public partial class ReadingTab : UserControl
    {
        public ReadingTab()
        {
            InitializeComponent();
            DataContext = this;
            var rtfPath = "c:\\users\\x.dr\\Desktop\\aaa.rtf";
            var contentts = File.ReadAllText(rtfPath);
            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(contentts));

            rtbMain.Selection.Load(stream, DataFormats.Rtf);
            rtbMain.Selection.Select(rtbMain.Document.ContentEnd, rtbMain.Document.ContentEnd);

        }
    }
}
