using GGPlayer.Pages;
using Shared;
using System.Windows;

namespace GGPlayer
{
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();
            CurrentFrame.Navigate(new MainPage());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(String.Format(Translations.GetValue("AreYouSureToExit")), "Good Grades", MessageBoxButton.YesNo, MessageBoxImage.Information) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}