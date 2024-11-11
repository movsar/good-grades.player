using System.Threading;
using System.Windows;

namespace Shared
{
    public partial class DownloadProgressWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public DownloadProgressWindow(CancellationTokenSource cancellationTokenSource)
        {
            InitializeComponent();
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void UpdateProgress(double progress)
        {
            DownloadProgressBar.Value = progress;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            Close();
        }
    }
}
