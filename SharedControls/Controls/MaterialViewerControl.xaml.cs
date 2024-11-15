using Plugin.SimpleAudioPlayer;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Path = System.IO.Path;

namespace Shared.Controls
{
    public partial class MaterialViewerControl : UserControl
    {
        private bool _isWebViewReady = false;
        private string materialTitle;
        #region Initialization
        public MaterialViewerControl()
        {
            DataContext = this;
            InitializeComponent();
        }
        #endregion

        public void Initialize(string title, byte[] data, byte[]? audio)
        {
            if (materialTitle == title)
            {
                return;
            }
            materialTitle = title;

            CrossSimpleAudioPlayer.Current.Stop();
            PurgeCache();

            if (audio != null)
            {
                CrossSimpleAudioPlayer.Current.Load(new MemoryStream(audio));
                spAudioControls.Visibility = Visibility.Visible;
            }

            webView.LoadPdfAsync(data);
        }

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