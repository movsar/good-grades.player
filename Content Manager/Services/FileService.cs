using System;
using System.IO;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;

namespace Content_Manager.Services
{
    public class FileService
    {
        public string AppResourcesPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GoodGrades" );
        internal void SetResourceString(string key, string value, string fileName = "Settings.resx")
        {
            string filePath = Path.Combine(AppResourcesPath, fileName);

            if (!Directory.Exists(AppResourcesPath))
            {
                Directory.CreateDirectory(AppResourcesPath);
            }

            using ResourceWriter resourceWriter = new ResourceWriter(filePath);
            resourceWriter.AddResource(key, value);
        }
        internal string ReadResourceString(string key, string fileName = "Settings.resx")
        {
            string filePath = Path.Combine(AppResourcesPath, fileName);
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            using ResourceReader resourceReader = new ResourceReader(Path.Combine(AppResourcesPath, fileName));
            string dataType = null;
            byte[] data = null;
            resourceReader.GetResourceData("lastOpenedDatabasePath", out dataType, out data);
            using BinaryReader reader = new BinaryReader(new MemoryStream(data));
            return reader.ReadString();
        }

        internal static string OpenFilePath(string filter)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = filter;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            return (result == false) ? "" : dialog.FileName;
        }

        internal static string SaveFilePath(string filter)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = filter;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            return (result == false) ? "" : dialog.FileName;
        }
    }
}
