using Data;
using Data.Entities;
using System.Windows.Controls;
using System.Windows.Input;
using Data.Services;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using GGPlayer.Services;
using System.Windows.Navigation;

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
        private readonly SegmentPage _segmentPage;
        private readonly ShellNavigationService _navigationService;

        public MainPage(ShellNavigationService navigationService, Storage storage, SegmentPage segmentPage)
        {
            DataContext = this;

            _storage = storage;
            _segmentPage = segmentPage;
            _navigationService = navigationService;

            // Intialize the visual elements
            InitializeComponent();
        }

        public void LoadContent()
        {
            // Load Segments into the collection view
            foreach (var segment in _storage.DbContext.Segments)
            {
                Segments.Add(segment);
            };

            // Set the Title based on current database
            _dbInfo = _storage.DbContext.DbMetas.First();
            DbTitle = _dbInfo.Title;
            DbDescription = _dbInfo.Description ?? string.Empty;
        }

        private void NavigateToSelectedSegment()
        {
            var selectedSegment = (Segment)lvSegments.SelectedItem;
            if (selectedSegment == null)
            {
                return;
            }
            _segmentPage.LoadSegment(selectedSegment);
            _navigationService.NavigateTo(_segmentPage);
        }

        #region Event handlers

        private void lvSegments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigateToSelectedSegment();
        }

        private void lvSegments_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                NavigateToSelectedSegment();
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
