using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Shared.Services
{
    public static class CultureService
    {
        public static void RunPowerShellScriptToRegisterCulture(string scriptPath)
        {
            // Check if the culture already exists
            if (CultureInfo.GetCultures(CultureTypes.AllCultures).Any(c => c.Name.Equals("ce", StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                };

                using (var process = Process.Start(processInfo))
                {
                    process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                    process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to execute PowerShell script. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
