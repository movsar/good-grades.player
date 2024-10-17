using Microsoft.Web.WebView2.Core;
using Plugin.SimpleAudioPlayer;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        }
        public MaterialViewer(string title, byte[] data)
        {
            Title = title;

            SharedInitialization();
            webView.LoadPdfAsync(data);
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

            webView.LoadPdfAsync(data);
        }
        #endregion

        #region AudioControls
        private void btnStop_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == btnStop)
            {
                CrossSimpleAudioPlayer.Current.Stop();
            }
        }

        private void btnPause_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CrossSimpleAudioPlayer.Current.Pause();
        }

        private void btnPlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CrossSimpleAudioPlayer.Current.Play();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            CrossSimpleAudioPlayer.Current.Stop();
            CrossSimpleAudioPlayer.Current.Dispose();
            base.OnClosing(e);
        }
        #endregion

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
