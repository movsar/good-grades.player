using Data;
using Data.Entities;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;

namespace Content_Player
{
    public partial class MainWindow : Window
    {
        public List<Segment> Segments { get; }
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
        }
    }
}