using Shared;

namespace GGManager.Services
{
    public class FileService
    {
        internal static string SelectImageFilePath()
        {
            return OpenFilePath(Translations.GetValue("ImageFiles"), Translations.GetValue("ImageFileChoose"));
        }
        internal static string SelectDatabaseFilePath()
        {
            return OpenFilePath(Translations.GetValue("DBFiles"), Translations.GetValue("DBFileChoose"));
        }
        internal static string SelectPdfFilePath()
        {
            return OpenFilePath(Translations.GetValue("PdfFiles"), Translations.GetValue("ChooseMaterialFile"));
        }
        internal static string SelectAudioFilePath()
        {
            return OpenFilePath(Translations.GetValue("AudioFiles"), Translations.GetValue("AudioFileChoose"));
        }

        internal static string OpenFilePath(string filter, string title)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = title;
            //dialog.Tag = tag;
            dialog.Filter = filter;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            return (result == false) ? "" : dialog.FileName;
        }

        internal static string SaveFilePath(string filter, string title)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Title = title;
            dialog.Filter = filter;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            return (result == false) ? "" : dialog.FileName;
        }

        internal static string SelectNewDatabaseFilePath()
        {
            return SaveFilePath(Translations.GetValue("DBFiles"), Translations.GetValue("DBFileChoose"));
        }
    }
}
