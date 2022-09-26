using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Services
{
    internal class FileService
    {
        internal static string SelectFilePath()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "RTF документы (.rtf)|*.rtf";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            return (result == false) ? "" : dialog.FileName;
        }
    }
}
