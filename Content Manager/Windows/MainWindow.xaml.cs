using Content_Manager.Services;
using Content_Manager.Stores;
using System.Windows;

namespace Content_Manager
{
    public partial class MainWindow : Window
    {
        private readonly ContentStore _contentStore;
        public MainWindow(ContentStore contentStore)
        {
            InitializeComponent();
            DataContext = this;
            _contentStore = contentStore;
            _contentStore.ContentStoreInitialized += ContentStoreInitialized;
            _contentStore.SelectedSegmentChanged += SelectedSegmentChanged;
        }

        private void SelectedSegmentChanged(Data.Models.Segment obj)
        {
            lblChooseSegment.Visibility = Visibility.Visible;
            ucSegmentControl.Visibility = Visibility.Visible;
        }

        private void ContentStoreInitialized()
        {
            lblChooseDb.Visibility = Visibility.Collapsed;
            lblChooseSegment.Visibility = Visibility.Visible;
            ucSegmentList.Visibility = Visibility.Visible;
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
    }
}
