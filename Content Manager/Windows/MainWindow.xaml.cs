using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Content_Manager.Windows;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Logging;
using Squirrel;
using Squirrel.Sources;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Content_Manager
{
    public partial class MainWindow : Window
    {
        private readonly ContentStore _contentStore;
        private readonly FileService _fileService;
        private ILogger _logger;

        private readonly string _appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public MainWindow(ContentStore contentStore, FileService fileService, ILogger<MainWindow> logger)
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = contentStore;
            _fileService = fileService;
            _logger = logger;

            // Open last opened database
            var lastOpenedDatabasePath = _fileService.ReadResourceString("lastOpenedDatabasePath");
            if (string.IsNullOrEmpty(lastOpenedDatabasePath) || !File.Exists(lastOpenedDatabasePath))
            {
                return;
            }
            _contentStore.OpenDatabase(lastOpenedDatabasePath);
        }

        private void OnDatabaseUpdated()
        {
        }

        //private void _contentStore_ItemUpdated(string interfaceName, IModelBase model)
        //{
        //    if (!interfaceName.Equals(nameof(IDbMeta)))
        //    {
        //        return;
        //    }

        //    var dbMeta = model as IDbMeta;

        //    SetTitle(dbMeta!.Title);
        //}

        private void SelectedSegmentChanged(SegmentEntity segment)
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

        private void ContentStoreInitialized()
        {
            lblChooseDb.Visibility = Visibility.Collapsed;
            lblChooseSegment.Visibility = Visibility.Visible;
            ucSegmentList.Visibility = Visibility.Visible;
            mnuDatabaseInfo.IsEnabled = true;

            SetTitle();
        }

        private void SetTitle(string? title = null)
        {
            Title = $"Good Grades | {_appVersion} | {title ?? _contentStore.Database.All<DbMeta>().First().Title}";
        }

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

        private static void OnAppInstall(SemanticVersion version, IAppTools tools)
        {
            tools.CreateShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppUninstall(SemanticVersion version, IAppTools tools)
        {
            tools.RemoveShortcutForThisExe(ShortcutLocation.StartMenu | ShortcutLocation.Desktop);
        }

        private static void OnAppRun(SemanticVersion version, IAppTools tools, bool firstRun)
        {
            tools.SetProcessAppUserModelId();
            // show a welcome message when the app is first installed
            if (firstRun) MessageBox.Show("Thanks for installing my application!");
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // run Squirrel first, as the app may exit after these run
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: OnAppInstall,
                onAppUninstall: OnAppUninstall,
                onEveryRun: OnAppRun);
        }
        private async Task UpdateMyApp()
        {
            var repositoryUrl = "https://github.com/movsar/good-grades";
            IUpdateSource girHubSource = new GithubSource(repositoryUrl, "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", true);
            string tmpLocalSource = @"D:\temp\content-manager";

            try
            {
                using var mgr = new UpdateManager(girHubSource);
                var updateInfo = await mgr.CheckForUpdate();

                var newVersion = updateInfo.FutureReleaseEntry.Version;
                var currentVersion = updateInfo.CurrentlyInstalledVersion.Version;

                _logger.LogInformation($"Before Checking for updates");

                _logger.LogDebug($"future: {newVersion}");
                _logger.LogDebug($"current: {currentVersion}");
                _logger.LogDebug($"");

                if (updateInfo?.FutureReleaseEntry != null && newVersion.CompareTo(currentVersion) <= 0)
                {
                    MessageBox.Show($"Установлена последняя версия!", "Good Grades");
                    return;
                }

                if (MessageBox.Show($"Доступна новая версия {newVersion}, обновить? Это может занять несколько минут", "Good Grades", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    IsEnabled = false;
                    await mgr.UpdateApp();
                    UpdateManager.RestartApp();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An error occurred when trying to update the app");
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        private async void mnuCheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            await UpdateMyApp();
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
    }
}
