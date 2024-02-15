using Data.Entities;
using Data;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using Data.Services;

namespace Content_Player.Pages
{
    public partial class MainPage : Page
    {
        public string DbTitle { get; }
        public List<Segment> Segments { get; }

        private readonly SettingsService _settingsService;
        private readonly Storage _storage;
        private readonly DbMeta _dbInfo;
        public MainPage()
        {
            // Initialize fields
            _settingsService = App.AppHost!.Services.GetRequiredService<SettingsService>();
            _storage = App.AppHost!.Services.GetRequiredService<Storage>();

            // Get the database path
            var dbAbsolutePath = _settingsService.GetValue("lastOpenedDatabasePath");
            if (string.IsNullOrEmpty(dbAbsolutePath) || !File.Exists(dbAbsolutePath))
            {
                dbAbsolutePath = GetDatabasePath();
            }
            _settingsService.SetValue("lastOpenedDatabasePath", dbAbsolutePath);
            _storage.SetDatabaseConfig(dbAbsolutePath);

            // Load Segments into the collection view
            Segments = _storage.Database.All<Segment>().ToList();

            // Set the Title based on current database
            _dbInfo = _storage.Database.All<DbMeta>().First();
            DbTitle = _dbInfo.Title;

            // Intialize the visual elements
            DataContext = this;
            InitializeComponent();
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
    }
}
