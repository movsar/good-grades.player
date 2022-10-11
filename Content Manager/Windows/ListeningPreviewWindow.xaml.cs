﻿using Data.Models;
using Plugin.SimpleAudioPlayer;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Content_Manager.Windows {
    public partial class ListeningPreviewWindow : Window {
        int index;
        readonly string _path;

        public ListeningPreviewWindow(ListeningMaterial listeningMaterial) {
            PurgeAudioFiles();
            CrossSimpleAudioPlayer.Current.Load(new MemoryStream(listeningMaterial.Audio));

            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.StreamSource = new MemoryStream(listeningMaterial.Image);
            logo.EndInit();

            imgMain.Source = logo;
            txtMain.Text = listeningMaterial.Content;
        }

        private void PurgeAudioFiles() {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            var potentialFileNames = Enumerable.Range(0, 20).Select(number => $"{number}.wav");

            foreach (var file in Directory.GetFiles(path, "*.wav")) {
                if (potentialFileNames.Contains(Path.GetFileName(file))) {
                    File.Delete(file);
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            //do my stuff before closing
            PurgeAudioFiles();
            base.OnClosing(e);
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e) {
            CrossSimpleAudioPlayer.Current.Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e) {
            CrossSimpleAudioPlayer.Current.Stop();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e) {
            CrossSimpleAudioPlayer.Current.Pause();
        }
    }
}
