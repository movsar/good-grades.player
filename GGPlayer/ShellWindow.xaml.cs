using GGPlayer.Pages;
using GGPlayer.Pages.Assignments;
using GGPlayer.Services;
using Shared;
using Shared.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

namespace GGPlayer
{
    public partial class ShellWindow : Window
    {
        private readonly StartWindow _startWindow;
        private readonly ShellNavigationService _navigationService;
        private readonly MainPage _mainPage;

        public ShellWindow(ShellNavigationService navigationService, MainPage mainPage)
        {
            InitializeComponent();

            _mainPage = mainPage;
            _mainPage.Initialize();

            _navigationService = navigationService;
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
            var isMainPage = nameof(MainPage) == e.Content.GetType().Name;
            if (isMainPage || !_navigationService.CanGoBack)
            {
                BackButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                BackButton.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_navigationService.CanGoBack)
            {
                return;
            }

            var originatingType = CurrentFrame.Content.GetType();
            switch (originatingType.Name)
            {
                case nameof(SegmentPage):
                    _navigationService.NavigateTo<MainPage>();
                    break;

                case nameof(MaterialViewerPage):
                    _navigationService.NavigateTo<SegmentPage>();
                    break;

                case nameof(AssignmentsPage):
                    _navigationService.NavigateTo<SegmentPage>();
                    break;

                case nameof(AssignmentViewerPage):
                    _navigationService.NavigateTo<AssignmentsPage>();
                    break;
            }
        }
    }
}
