using GGPlayer.Pages;
using GGPlayer.Services;
using Shared;
using Shared.Controls;
using System.Windows;
using System.Windows.Navigation;

namespace GGPlayer
{
    public partial class ShellWindow : Window
    {
        private readonly ShellNavigationService _navigationService;
        private readonly MainPage _mainPage;

        public ShellWindow(ShellNavigationService navigationService, MainPage mainPage)
        {
            InitializeComponent();

            _navigationService = navigationService;
            _mainPage = mainPage;
            _mainPage.LoadContent();
            _navigationService.Initialize(CurrentFrame);

            _navigationService.NavigateTo(_mainPage);
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
            BackButton.Visibility = _navigationService.CanGoBack ? Visibility.Visible : Visibility.Hidden;
        }

        private void BackButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var isClickedOnMainPage = CurrentFrame.Content.GetType().Name == nameof(MainPage);

            if (_navigationService.CanGoBack && !isClickedOnMainPage)
            {
                _navigationService.NavigateTo(_mainPage);
            }
            else
            {
                BackButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
