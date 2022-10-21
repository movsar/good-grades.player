using Data.Interfaces;
using Data.Models;
using Plugin.SimpleAudioPlayer;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Content_Manager.Windows
{
    public partial class ListeningPreviewWindow : Window
    {
        private void LoadDocument(string rtf, byte[] image)
        {
            if (image != null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.StreamSource = new MemoryStream(image);
                logo.EndInit();

                imgMain.Source = logo;
            }

            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(rtf));
            var txtRange = new TextRange(flowParagraph.ContentStart, flowParagraph.ContentEnd);
            txtRange.Load(stream, DataFormats.Rtf);
        }
        public ListeningPreviewWindow(string lmTitle, string lmText, byte[] lmImage, byte[] lmAudio)
        {
            PurgeAudioFiles();
            CrossSimpleAudioPlayer.Current.Load(new MemoryStream(lmAudio));

            InitializeComponent();

            LoadDocument(lmText, lmImage);
        }

        private void PurgeAudioFiles()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            var potentialFileNames = Enumerable.Range(0, 20).Select(number => $"{number}.wav");

            foreach (var file in Directory.GetFiles(path, "*.wav"))
            {
                if (potentialFileNames.Contains(Path.GetFileName(file)))
                {
                    File.Delete(file);
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //do my stuff before closing
            CrossSimpleAudioPlayer.Current.Stop();
            CrossSimpleAudioPlayer.Current.Dispose();
            PurgeAudioFiles();
            base.OnClosing(e);
        }

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
    }
}
