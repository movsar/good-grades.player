using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Content_Manager.Windows;
using Data.Interfaces;
using System.Collections;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Input;

namespace Content_Manager
{
    public partial class MainWindow : Window
    {
        private readonly ContentStore _contentStore;
        private readonly FileService _fileService;
        public MainWindow(ContentStore contentStore, FileService fileService)
        {
            InitializeComponent();
            DataContext = this;
            _contentStore = contentStore;
            _fileService = fileService;

            _contentStore.ContentStoreInitialized += ContentStoreInitialized;
            _contentStore.SelectedSegmentChanged += SelectedSegmentChanged;
            _contentStore.ItemUpdated += _contentStore_ItemUpdated;

            // Open last opened database
            var lastOpenedDatabasePath = _fileService.ReadResourceString("lastOpenedDatabasePath");
            if (string.IsNullOrEmpty(lastOpenedDatabasePath))
            {
                return;
            }
            _contentStore.OpenDatabase(lastOpenedDatabasePath);
        }

        private void _contentStore_ItemUpdated(string interfaceName, IModelBase model)
        {
            if (!interfaceName.Equals(nameof(IDbMeta)))
            {
                return;
            }

            var dbMeta = model as IDbMeta;

            SetTitle(dbMeta!.Title);
        }

        private void SelectedSegmentChanged(Data.Models.Segment obj)
        {
            if (obj != null)
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
            Title = $"Good Grades | {title ?? _contentStore.GetDbMeta().Title}";
        }

        private void mnuOpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.OpenFilePath("Файлы Баз Данных (.sgb) | *.sgb;");
            if (string.IsNullOrEmpty(filePath)) return;

            _contentStore.OpenDatabase(filePath);
        }

        private void mnuCreateDatabase_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SaveFilePath("Файлы Баз Данных (.sgb) | *.sgb;");
            if (string.IsNullOrEmpty(filePath)) return;

            _contentStore.CreateDatabase(filePath);
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

    }
}
