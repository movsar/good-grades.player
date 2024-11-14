
using Data;
using Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Shared;
using Shared.Services;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GGPlayer
{
    public partial class StartWindow : Window
    {
        private readonly SettingsService _settingsService;
        private readonly Storage _storage;
        private readonly ShellWindow _shell;

        public StartWindow(SettingsService settingsService, Storage storage)
        {
            InitializeComponent();
            DataContext = this;

            _settingsService = settingsService;
            _storage = storage;

            try
            {
                LoadDatabase();
                _shell = App.AppHost!.Services.GetRequiredService<ShellWindow>();
            }
            catch (OperationCanceledException)
            {
                // Do nothing, user cancelled 
            }
            catch (Exception ex)
            {
                ExceptionService.HandleError(ex, ex.Message);
            }
        }
        public void LoadDatabase(bool restoreLatest = true)
        {
            // Get the database path
            var dbAbsolutePath = _settingsService.GetValue("lastOpenedDatabasePath");
            if (!restoreLatest || string.IsNullOrEmpty(dbAbsolutePath) || !File.Exists(dbAbsolutePath))
            {
                dbAbsolutePath = GetDatabasePath();
            }

            // If the user cancels and closes the window
            if (string.IsNullOrEmpty(dbAbsolutePath))
            {
                return;
            }

            //открытие последней открытой БД
            _settingsService.SetValue("lastOpenedDatabasePath", dbAbsolutePath);
            _storage.SetDatabaseConfig(dbAbsolutePath);
            btnGo.IsEnabled = true;

            // Set the background image for the class
            var dbMeta = _storage.DbContext.DbMetas.First();
            Title = "Good Grades: " + dbMeta.Title;
            if (dbMeta.BackgroundImage?.Length > 0)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.StreamSource = new MemoryStream(dbMeta.BackgroundImage);
                logo.EndInit();
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = logo;
                pnlMain.Background = myBrush;
            }
        }
        private string GetDatabasePath()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = Translations.GetValue("DBFiles");
            ofd.Multiselect = false;
            var result = ofd.ShowDialog();
            if (result.HasValue)
            {
                return ofd.FileName;
            }

            MessageBox.Show(Translations.GetValue("DBFileChoose"));
            return GetDatabasePath();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Создание и показ основного окна
            _shell.Show();

            // Закрытие стартового окна
            Hide();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void CloseProgram_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            LoadDatabase(false);
        }

        private void mnuSetLanguageChechen_Click(object sender, RoutedEventArgs e)
        {
            _settingsService.SetValue("uiLanguageCode", "uk");
            Translations.SetToCulture("uk");
            Translations.RestartApp();
        }

        private void mnuSetLanguageRussian_Click(object sender, RoutedEventArgs e)
        {
            _settingsService.SetValue("uiLanguageCode", "ru");
            Translations.SetToCulture("ru");
            Translations.RestartApp();
        }

        private async void mnuCheckUpdates_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            await UpdateService.UpdateMyApp("player");
            IsEnabled = true;
        }
    }
}
