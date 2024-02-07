using Data;
using Data.Entities;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Content_Player
{
    public partial class MainWindow : Window
    {
        public List<Segment> Segments { get; }
        public string DbTitle { get; }
        public MainWindow(Storage storage)
        {
            InitializeComponent();
            DataContext = this;

            string dbRelativePath = @".\..\..\..\..\Documentation\GoodGradesDB.sgb";
            var fi = new FileInfo(dbRelativePath);
            var dbAbsolutePath = fi.FullName;

            storage.SetDatabaseConfig(dbAbsolutePath);
            var db = storage.Database;
            Segments = db.All<Segment>().ToList();
            var meta = db.All<DbMeta>().First();
            DbTitle = meta.Title;
        }
        private void LoadSegment()
        {
            var selectedSegment = (Segment)lvSegments.SelectedItem;
            if (selectedSegment == null)
            {
                return;
            }
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