using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utilities
{
    public static class FontInstaller
    {
        private static readonly string FontsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));

        private static readonly string FontFileName = "segmdl2.ttf";
        public static bool IsWindows7()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT &&
                   Environment.OSVersion.Version.Major == 6 &&
                   Environment.OSVersion.Version.Minor == 1;
        }

        public static bool IsFontInstalled()
        {
            return File.Exists(Path.Combine(FontsFolderPath, FontFileName));
        }

        public static void RunFontInstallScript()
        {
            if (IsFontInstalled())
            {
                Debug.WriteLine($"The font '{FontFileName}' is already installed.");
                return;
            }

            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Fonts", "InstallFont.bat");

            if (!File.Exists(scriptPath))
            {
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = scriptPath,
                    Verb = "runas",
                    UseShellExecute = true
                })?.WaitForExit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred while running the script: {ex.Message}");
            }
        }
    }
}