using System;
using System.Formats.Asn1;
using System.IO;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;

namespace Content_Manager.Services
{
    public class FileService
    {
        public string AppResourcesPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GoodGrades");
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

        internal static string SelectImageFilePath()
        {
            return OpenFilePath("Файлы изображений (.png) | *.png; *.jpg; *.jpeg; *.tiff", "Выбор файла с изображением");
        }
        internal static string SelectDatabaseFilePath()
        {
            return OpenFilePath("Файлы Баз Данных (.sgb) | *.sgb;", "Выбор файла баз данных");
        }
        internal static string SelectTextFilePath()
        {
            return OpenFilePath("Файлы с RTF текстом (.rtf) | *.rtf;", "Выбор файла с текстом");
        }
        internal static string SelectAudioFilePath()
        {
            return OpenFilePath("MP3 Файлы (.mp3) | *.mp3", "Выбор файла с аудиозаписью");
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
            return SaveFilePath("Файлы Баз Данных (.sgb) | *.sgb;", "Выбор файла баз данных");
        }
    }
}
