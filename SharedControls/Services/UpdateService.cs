using Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Velopack;

namespace Shared.Services
{
    public static class UpdateService
    {
        public static IHost? AppHost { get; set; }
        public static async Task UpdateMyApp(string module, bool ignoreSkipVersion = false, bool showSkipOption = false)
        {
            try
            {
                Log.Information($"Before Checking for updates");
                var latestRelease = await GitHubService.GetLatestRelease(module);
                var latestVersion = Regex.Match(latestRelease.Name!, @"\d+\.\d+\.\d+").Value.Trim();
                var setupAsset = latestRelease.Assets
                        .Where(a => a.Name!.ToLower().Contains("setup"))
                        .FirstOrDefault();

                var currentVersion = Assembly.GetEntryAssembly()?.GetName().Version;
                if (currentVersion == null)
                {
                    throw new Exception("Couldn't fetch current version information");
                }
                var currentVersionAsString = $"{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}".Trim();
               
                var settingsService = AppHost?.Services.GetRequiredService<SettingsService>();
                if (settingsService == null)
                {
                    throw new Exception("SettingsService is not available");
                }
                var skipVersion = settingsService.GetValue("SkipVersion");
                if (!ignoreSkipVersion && skipVersion == latestVersion)
                {
                    Log.Information("Update skipped due to SkipVersion.");
                    return;
                }

                if (currentVersionAsString.Equals(latestVersion))
                {
                    OkDialog.Show(Translations.GetValue("LastVersionInstalled"));
                    return;
                }

                var message = string.Format(Translations.GetValue("AvailableNewVersion"), latestVersion);

                MessageBoxResult result;
                if (showSkipOption == true)
                {
                    result = SkipYesNoDialog.Show(message);
                }
                else
                {
                    result = YesNoDialog.Show(message);
                }

                if (result == MessageBoxResult.No)
                {
                    return;
                }
                else if (result == MessageBoxResult.Cancel && showSkipOption)
                {
                    settingsService.SetValue("SkipVersion", latestVersion);
                    return;
                }

                // Download new version
                var filePath = await GitHubService.DownloadUpdate(setupAsset);
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    // Install new version and restart app
                    Process.Start(filePath);
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/movsar/good-grades/releases",
                    UseShellExecute = true,
                });
            }
        }
    }
}
