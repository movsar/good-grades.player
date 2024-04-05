using Shared.Translations;

namespace GGManager.Services
{
    public class FileService
    {
        internal static string SelectImageFilePath()
        {
            return OpenFilePath(Ru.ImageFiles, Ru.ImageFileChoose);
        }
        internal static string SelectDatabaseFilePath()
        {
            return OpenFilePath(Ru.DBFiles, Ru.DBFileChoose);
        }
        internal static string SelectTextFilePath()
        {
            return OpenFilePath(Ru.RtfFiles, Ru.RtfFileChoose);
        }
        internal static string SelectAudioFilePath()
        {
            return OpenFilePath(Ru.AudioFiles, Ru.AudioFileChoose);
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
            return SaveFilePath(Ru.DBFiles, Ru.DBFileChoose);
        }
    }
}
