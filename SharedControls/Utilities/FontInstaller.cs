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
        public static bool IsWindows7()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT &&
                   Environment.OSVersion.Version.Major == 6 &&
                   Environment.OSVersion.Version.Minor == 1;
        }

        public static void RunFontInstallScript()
        {
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