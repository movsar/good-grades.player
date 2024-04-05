using GGManager.Services;
using GGManager.Stores;
using GGManager.UserControls;
using GGManager.Windows;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Shared.Translations;
using Data.Services;
using Shared.Services;
using Velopack;
using Velopack.Sources;
using System.Diagnostics;

namespace GGManager
{
    public partial class MainWindow : Window
    {
        private readonly ContentStore _contentStore;
        private readonly SettingsService _settingsService;
        private readonly string _repositoryUrl = "https://github.com/movsar/good-grades";

        public MainWindow(ContentStore contentStore, SettingsService fileService)
        {
            InitializeComponent();
            DataContext = this;

            _settingsService = fileService;
            _contentStore = contentStore;
            _contentStore.SelectedSegmentChanged += SelectedSegmentChanged;
            _contentStore.CurrentDatabaseChanged += OnDatabaseOpened;

            var lastOpenedDatabasePath = _settingsService.GetValue("lastOpenedDatabasePath");
            if (!string.IsNullOrEmpty(lastOpenedDatabasePath) && File.Exists(lastOpenedDatabasePath))
            {
                _contentStore.OpenDatabase(lastOpenedDatabasePath);
            }
        }

        private void SelectedSegmentChanged(Segment segment)
        {
            if (segment != null)
            {
                lblChooseSegment.Visibility = Visibility.Hidden;

                ucSegmentControlParent.Children.Clear();
                ucSegmentControlParent.Children.Add(new SegmentControl());
            }
            else
            {
                ucSegmentControlParent.Children.Clear();
                lblChooseSegment.Visibility = Visibility.Visible;
            }
        }

        private void SetTitle(string? title = null)
        {
            string _appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = $"Good Grades | {_appVersion} | {title ?? _contentStore.DbContext.DbMetas.First().Title}";
        }

        #region Database Operations
        private void mnuOpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectDatabaseFilePath();
            if (string.IsNullOrEmpty(filePath)) return;

            _contentStore.OpenDatabase(filePath);
        }

        private void mnuCreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectNewDatabaseFilePath();
            if (string.IsNullOrEmpty(filePath)) return;

            _contentStore.CreateDatabase(filePath);

            Task.Delay(200);
            var dbInfo = new DbInfoWindow();
            dbInfo.ShowDialog();
        }

        private void mnuDatabaseInfo_Click(object sender, RoutedEventArgs e)
        {
            var dbInfoWindow = new DbInfoWindow();
            dbInfoWindow.ShowDialog();
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void OnDatabaseOpened()
        {
            _contentStore.SelectedSegment = _contentStore.DbContext.Segments.FirstOrDefault();

            lblChooseDb.Visibility = Visibility.Collapsed;
            lblChooseSegment.Visibility = Visibility.Visible;
            ucSegmentList.Visibility = Visibility.Visible;
            mnuDatabaseInfo.IsEnabled = true;

            SetTitle();
        }

        private void mnuImportDatabase_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectDatabaseFilePath();
            if (!File.Exists(filePath))
            {
                return;
            }
            _contentStore.ImportDatabase(filePath);
        }
        #endregion

        #region App Install, Update and Uninstall

        //private async Task UpdateMyApp()
        //{
        //    var repositoryUrl = "https://github.com/movsar/good-grades";
        //    IUpdateSource girHubSource = new GithubSource(repositoryUrl, "", false);
        //    string tmpLocalSource = @"D:\temp\content-manager";

        //    try
        //    {
        //        using var mgr = new UpdateManager(girHubSource);
        //        var updateInfo = await mgr.CheckForUpdate();

        //        var newVersion = updateInfo.FutureReleaseEntry.Version;
        //        var currentVersion = updateInfo.CurrentlyInstalledVersion.Version;

        //        //_logger.LogInformation($"Before Checking for updates");

        //        //_logger.LogDebug($"future: {newVersion}");
        //        //_logger.LogDebug($"current: {currentVersion}");
        //        //_logger.LogDebug($"");

        //        if (updateInfo?.FutureReleaseEntry != null && newVersion.CompareTo(currentVersion) <= 0)
        //        {
        //            MessageBox.Show(Ru.LastVersionInstalled, "Good Grades");
        //            return;
        //        }

        //        if (MessageBox.Show(String.Format(Ru.AvailableNewVersion, newVersion), "Good Grades", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
        //        {
        //            IsEnabled = false;
        //            await mgr.UpdateApp();
        //            UpdateManager.RestartApp();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionService.HandleError(ex, "Ошибка при попытаке обновления приложения");
        //    }
        //}

        private async Task UpdateMyApp()
        {
            //var token = "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";

            IUpdateSource girHubSource = new GithubSource(_repositoryUrl, "", false);
            var mgr = new UpdateManager(girHubSource);

            // Check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                return;
            }

            // Download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // Install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }

        private async void mnuCheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _repositoryUrl,
                UseShellExecute = true
            });
        }

        #endregion
    }
}
