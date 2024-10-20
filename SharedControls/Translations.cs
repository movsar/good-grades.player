using System;
using System.Globalization;
using System.Resources;
using System.Threading;

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
        }
    }
}
