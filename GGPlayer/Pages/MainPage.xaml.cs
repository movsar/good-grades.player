using Data;
using Data.Entities;
using System.Windows.Controls;
using System.Windows.Input;
using Data.Services;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Navigation;
using System.Windows;
using Shared.Controls;

namespace GGPlayer.Pages
{
    public partial class MainPage : Page
    {
        public string DbTitle { get; set; }
        public string DbDescription { get; set; }
        public ObservableCollection<Segment> Segments { get; set; } = new ObservableCollection<Segment>();

        private DbMeta _dbInfo;
        private readonly SettingsService _settingsService;
        private readonly Storage _storage;
        private readonly ShellWindow _shell;

        public MainPage(ShellWindow shellWindow)
        {
            DataContext = this;

            _storage = App.AppHost!.Services.GetRequiredService<Storage>();
            _shell = shellWindow;

            // Load Segments into the collection view
            foreach (var segment in _storage.DbContext.Segments)
            {
                Segments.Add(segment);
            };

            // Set the Title based on current database
            _dbInfo = _storage.DbContext.DbMetas.First();
            DbTitle = _dbInfo.Title;
            DbDescription = _dbInfo.Description ?? string.Empty;

            // Intialize the visual elements
            InitializeComponent();
        }

        private void LoadSegment()
        {
            var selectedSegment = (Segment)lvSegments.SelectedItem;
            if (selectedSegment == null)
            {
                return;
            }

            _shell.CurrentFrame.Navigate(new SegmentPage(_shell, selectedSegment));
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

        //private void mnuOpenDatabase_Click(object sender, RoutedEventArgs e)
        //{
        //    Segments.Clear();
        //    LoadDatabase(false);
        //}

    }
}
