using System.Net.Http.Headers;
using System.Reflection;
using System.Resources;

namespace Data.Services
{
    public class SettingsService
    {
        public string AppResourcesPath { get; }
        private string _fileName = "Settings.resx";
        public SettingsService()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyName = assembly.GetName().Name;
            AppResourcesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyName);
        }
        public void SetValue(string key, string value)
        {
            string filePath = Path.Combine(AppResourcesPath, _fileName);

            if (!Directory.Exists(AppResourcesPath))
            {
                Directory.CreateDirectory(AppResourcesPath);
            }

            using ResourceWriter resourceWriter = new ResourceWriter(filePath);
            resourceWriter.AddResource(key, value);
        }
        public string? GetValue(string key)
        {
            string filePath = Path.Combine(AppResourcesPath, _fileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            using ResourceReader resourceReader = new ResourceReader(Path.Combine(AppResourcesPath, _fileName));
            string dataType = null;
            byte[] data = null;
            try
            {

                resourceReader.GetResourceData(key, out dataType, out data);
                using BinaryReader reader = new BinaryReader(new MemoryStream(data));
                var value = reader.ReadString();

                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
            catch { }
            return null;
        }
    }
}
