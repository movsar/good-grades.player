using GGPlayer.Pages;
using Shared;
using System.Windows;
using Data.Entities;
using Microsoft.Win32;

namespace GGPlayer
{
    public partial class ShellWindow : Window
    {
        private MainPage _mainPage;
        public ShellWindow()
        {
            InitializeComponent();
            _mainPage = new MainPage();
            CurrentFrame.Navigate(new MainPage());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(String.Format(Translations.GetValue("AreYouSureToExit")), "Good Grades", MessageBoxButton.YesNo, MessageBoxImage.Information) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void OpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            _mainPage.LoadDatabase(false);

        }

        private void CloseProgram_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
