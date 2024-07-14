using Microsoft.Web.WebView2.Core;
using Plugin.SimpleAudioPlayer;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Shared.Viewers
{
    public partial class MaterialViewer : Window
    {
        private string _pdfBase64;
        private bool _isWebViewReady = false;

        #region Initialization
        private void SharedInitialization()
        {
            PurgeCache();
            InitializeComponent();
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }
        public MaterialViewer(string title, byte[] data)
        {
            Title = title;

            SharedInitialization();
            LoadPdfAsync(data);
        }

        public MaterialViewer(string title, byte[] data, byte[]? audio)
        {
            SharedInitialization();

            Title = title;

            if (audio != null)
            {
                CrossSimpleAudioPlayer.Current.Load(new MemoryStream(audio));
                spAudioControls.Visibility = Visibility.Visible;
            }

            LoadPdfAsync(data);
        }
        #endregion

        #region AudioControls
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            CrossSimpleAudioPlayer.Current.Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            CrossSimpleAudioPlayer.Current.Stop();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            CrossSimpleAudioPlayer.Current.Pause();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            CrossSimpleAudioPlayer.Current.Stop();
            CrossSimpleAudioPlayer.Current.Dispose();
            base.OnClosing(e);
        }
        #endregion
        private async void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                _isWebViewReady = true;
            }
            else
            {
                MessageBox.Show($"WebView2 initialization failed. Error: {e.InitializationException.Message}");
            }
        }

        private async Task LoadPdfAsync(byte[] pdfBytes)
        {
            try
            {
                await webView.EnsureCoreWebView2Async();

                // Convert PDF bytes to Base64 string
                string pdfBase64 = Convert.ToBase64String(pdfBytes);
                string pdfDataUri = $"data:application/pdf;base64,{pdfBase64}";

                // Navigate to the PDF data URI
                webView.CoreWebView2.Navigate(pdfDataUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load PDF: {ex.Message}");
            }
        }


        private void PurgeCache()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            var potentialFileNames = Enumerable.Range(0, 20).Select(number => $"{number}.wav");

            foreach (var file in Directory.GetFiles(path, "*.wav"))
            {
                if (potentialFileNames.Contains(Path.GetFileName(file)))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, ex.Message);
                    }
                }
            }
        }
    }
}
