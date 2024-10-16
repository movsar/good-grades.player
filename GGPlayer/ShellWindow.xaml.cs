using GGPlayer.Pages;
using Shared;
using System.Windows;
using System.Windows.Navigation;

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
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (CurrentFrame.CanGoBack)
            {
                CurrentFrame.GoBack();
            }
        }

        private void GoForward(object sender, RoutedEventArgs e)
        {
            if (CurrentFrame.CanGoForward)
            {
                CurrentFrame.GoForward();
            }
        }

        private void CurrentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // Update the button state after navigation
            BackButton.IsEnabled = CurrentFrame.CanGoBack;
            ForwardButton.IsEnabled = CurrentFrame.CanGoForward;
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
