using System;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Shared
{
    public static class Translations
    {
        static Translations()
        {
            //SetToCulture("ce");
        }

        public static string GetValue(string key)
        {
            // Create a ResourceManager for the Resources file
            ResourceManager rm = new ResourceManager("Shared.GgLocalization", typeof(Translations).Assembly);

            var v = rm.GetString(key);
            return string.IsNullOrWhiteSpace(v) ? string.Empty : v;
        }

        public static void SetToCulture(string code)
        {
            CultureInfo cultureInfo = new CultureInfo(code);

            // Set both the current culture and the current UI culture
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ApplyLanguageResources();
        }

        private static void ApplyLanguageResources()
        {
            // Clear existing resource dictionaries
            var dicts = Application.Current.Resources.MergedDictionaries;
            dicts.Clear();

            // Load the new language resource file
            var resourceDict = new ResourceDictionary
            {
                Source = new Uri($"Resources/Resources.GgLocalization.{Thread.CurrentThread.CurrentUICulture.Name}.xaml", UriKind.Relative)
            };

            // Add the new resource dictionary
            dicts.Add(resourceDict);

            // Refresh all open windows to apply the language change
            foreach (Window window in Application.Current.Windows)
            {
                window.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentUICulture.IetfLanguageTag);
                // You may need to trigger a manual refresh for data-bound controls here
                // If necessary, recreate or reload window contents
            }
        }
    }
}
