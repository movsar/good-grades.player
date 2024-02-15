using Data.Entities;
using Data;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Content_Player.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = this;

            string dbRelativePath = @".\..\..\..\..\Documentation\GoodGradesDB.sgb";
            var fi = new FileInfo(dbRelativePath);
            var dbAbsolutePath = fi.FullName;

            var storage = App.AppHost!.Services.GetRequiredService<Storage>();
            storage.SetDatabaseConfig(dbAbsolutePath);
            var db = storage.Database;
            Segments = db.All<Segment>().ToList();
            var meta = db.All<DbMeta>().First();
            DbTitle = meta.Title;
        }
        public List<Segment> Segments { get; }
        public string DbTitle { get; }
      
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
