using GGPlayer.Pages;
using Shared;
using Shared.Controls;
using System.Windows;
using System.Windows.Navigation;

namespace GGPlayer
{
    public partial class ShellWindow : Window
    {
        public ShellWindow()
        {
            InitializeComponent();

            var _mainPage = new MainPage(this);
            CurrentFrame.Navigate(_mainPage);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (YesNoDialog.Show(Translations.GetValue("AreYouSureToExit")) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void CurrentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            BackButton.Visibility = Visibility.Hidden;
        }

        private void CurrentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            BackButton.Visibility = CurrentFrame.CanGoBack ? Visibility.Visible : Visibility.Hidden;
        }

        private void BackButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var isClickedOnMainPage = CurrentFrame.Content.GetType().Name == nameof(MainPage);
            if (CurrentFrame.CanGoBack && !isClickedOnMainPage)
            {
                CurrentFrame.Navigate(new MainPage(this));
            }
            else
            {
                BackButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
