using Data.Models;
using Plugin.SimpleAudioPlayer;
using System.IO;
using System.Windows;

namespace Content_Manager.Windows
{
    public partial class ListeningPreviewWindow : Window
    {
        public ListeningPreviewWindow(ListeningMaterial listeningMaterial)
        {
            InitializeComponent();
            CrossSimpleAudioPlayer.Current.Load(new MemoryStream(listeningMaterial.Audio));
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
