using Data;
using Data.Entities;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using Data.Services;
using System.Collections.ObjectModel;
using Shared.Services;

namespace GGPlayer.Pages
{
    public partial class MainPage : Page
    {
        public string DbTitle { get; set; }
        public ObservableCollection<Segment> Segments { get; set; } = new ObservableCollection<Segment>();

        private DbMeta _dbInfo;
        private readonly SettingsService _settingsService;
        private readonly Storage _storage;
        public MainPage()
        {
            // Initialize fields
            _settingsService = App.AppHost!.Services.GetRequiredService<SettingsService>();
            _storage = App.AppHost!.Services.GetRequiredService<Storage>();

            try
            {
                LoadDatabase();
            }
            catch (Exception ex)
            {
                ExceptionService.HandleError(ex, ex.Message);
            }

            // Intialize the visual elements
            DataContext = this;
            InitializeComponent();
        }

        private void LoadDatabase(bool restoreLatest = true)
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

            _settingsService.SetValue("lastOpenedDatabasePath", dbAbsolutePath);
            _storage.SetDatabaseConfig(dbAbsolutePath);

            // Load Segments into the collection view
            foreach (var segment in _storage.DbContext.Segments)
            {
                Segments.Add(segment);
            };

            // Set the Title based on current database
            _dbInfo = _storage.DbContext.DbMetas.First();
            DbTitle = _dbInfo.Title;
        }
        private string GetDatabasePath()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = Shared.Translations.Ru.DBFiles;
            ofd.Multiselect = false;
            var result = ofd.ShowDialog();
            if (result.HasValue)
            {
                return ofd.FileName;
            }

            MessageBox.Show(Shared.Translations.Ru.DBFileChoose);
            return GetDatabasePath();
        }

        private void LoadSegment()
        {
            var selectedSegment = (Segment)lvSegments.SelectedItem;
            if (selectedSegment == null)
            {
                return;
            }

            this.NavigationService.Navigate(new SegmentPage(selectedSegment));
        }

        #region Event handlers
        private void lvSegments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LoadSegment();
        }
        private void lvSegments_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                LoadSegment();
            }
        }
        #endregion

        private void mnuOpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            Segments.Clear();
            LoadDatabase(false);
        }

    }
}
