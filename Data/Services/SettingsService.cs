using System.Reflection;
using System.Text.Json;

namespace Data.Services
{
    public class SettingsService
    {
        private string _settingsFile = "Settings.json";
        public string AppResourcesPath { get; }
        private string _filePath;

        public SettingsService()
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblyName = assembly.GetName().Name;

            AppResourcesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Good Grades", assemblyName);
            _filePath = Path.Combine(AppResourcesPath, _settingsFile);

            if (!Directory.Exists(AppResourcesPath))
            {
                Directory.CreateDirectory(AppResourcesPath);
            }
        }

        public void SetValue(string key, string value)
        {
            var settings = LoadSettings();
            settings[key] = value;
            SaveSettings(settings);
        }

        public string? GetValue(string key)
        {
            var settings = LoadSettings();
            return settings.ContainsKey(key) ? settings[key] : null;
        }

        private Dictionary<string, string> LoadSettings()
        {
            if (!File.Exists(_filePath)) return new Dictionary<string, string>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }

        private void SaveSettings(Dictionary<string, string> settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
