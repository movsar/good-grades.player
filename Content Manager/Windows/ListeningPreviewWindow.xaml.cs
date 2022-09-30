using Data.Models;
using Plugin.SimpleAudioPlayer;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Content_Manager.Windows
{
    public partial class ListeningPreviewWindow : Window
    {
        public ListeningPreviewWindow(ListeningMaterial listeningMaterial)
        {
            CrossSimpleAudioPlayer.Current.Load(new MemoryStream(listeningMaterial.Audio));
        
            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.StreamSource = new MemoryStream(listeningMaterial.Image);
            logo.EndInit();

            imgMain.Source = logo;
            txtMain.Text = listeningMaterial.Content;
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
