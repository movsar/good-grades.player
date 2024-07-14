using System.Globalization;
using System.Resources;
using System.Threading;

namespace Shared
{
    public static class Translations
    {
        static Translations()
        {
            CultureInfo culture = new CultureInfo("ce");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public static string GetValue(string key)
        {
            // Create a ResourceManager for the Resources file
            ResourceManager rm = new ResourceManager("Shared.GgLocalization", typeof(Translations).Assembly);

            var v = rm.GetString(key);
            return string.IsNullOrWhiteSpace(v) ? string.Empty : v;
        }
    }
}
