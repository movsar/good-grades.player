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
        public static async Task UpdateMyApp(string releasesUrl)
        {
            try
            {
                Log.Information($"Before Checking for updates");
                var latestRelease = await GitHubService.GetLatestRelease();
                var latestVersion = Regex.Match(latestRelease.Name!, @"\d+\.\d+\.\d+").Value.Trim();
                var setupAsset = latestRelease.Assets
                        .Where(a => a.Name!.ToLower().Contains("setup"))
                        .FirstOrDefault();

                var currentVersion = Assembly.GetEntryAssembly()?.GetName().Version;
                if (currentVersion == null)
                {
                    throw new Exception("Couldn't fetch current version information");
                }
                var currentVersionAsString = $"{currentVersion.Major}.{currentVersion.MinorRevision}.{currentVersion.Build}".Trim();
                if (currentVersionAsString.Equals(latestVersion))
                {
                    OkDialog.Show(Translations.GetValue("LastVersionInstalled"));
                    return;
                }

                if (YesNoDialog.Show(string.Format(Translations.GetValue("AvailableNewVersion"), latestVersion)) != MessageBoxResult.Yes)
                {
                    return;
                }

                // Download new version
                var filePath = await GitHubService.DownloadUpdate(setupAsset);

                // Install new version and restart app
                Process.Start(filePath);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleError(ex, "Ошибка при попытаке обновления приложения");
            }
        }
    }
}
