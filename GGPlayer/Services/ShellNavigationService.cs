using GGPlayer.Pages.Assignments;
using GGPlayer.Pages;
using Microsoft.Extensions.DependencyInjection;
using Plugin.SimpleAudioPlayer;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace GGPlayer.Services
{
    public class ShellNavigationService
    {
        public event Action<Page> Navigated;
        private Frame _frame;
        private Page _currentPage;
        public bool CanGoBack => _frame?.CanGoBack ?? false;
        public bool IsNavigating { get; set; } = false;
        public void Initialize(Frame frame)
        {
            _frame = frame;
            _frame.Navigated += _frame_Navigated;
            _frame.Navigating += _frame_Navigating;
            _frame.NavigationStopped += _frame_NavigationStopped;
        }

        private void _frame_NavigationStopped(object sender, NavigationEventArgs e)
        {
            IsNavigating = false;
        }

        private void _frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            IsNavigating = true;
        }

        public void NavigateTo(Page page)
        {
            _currentPage = page;
            _frame.Navigate(page);
        }

        private void _frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsNavigating = false;
            Navigated?.Invoke(_currentPage);
        }

        public void NavigateTo<T>() where T : Page
        {
            var page = App.AppHost!.Services.GetRequiredService<T>();
            NavigateTo(page);
        }

        public void GoBack(string? originatingTypeName)
        {
            if (!CanGoBack)
            {
                return;
            }

            if (originatingTypeName == null)
            {
                _frame.GoBack();
                return;
            }

            // Custom overrides for the back navigation
            switch (originatingTypeName)
            {
                case nameof(MainPage):
                    NavigateTo<StartPage>();
                    break;

                case nameof(SegmentPage):
                    NavigateTo<MainPage>();
                    break;

                case nameof(MaterialViewerPage):
                    CrossSimpleAudioPlayer.Current.Stop();
                    NavigateTo<SegmentPage>();
                    break;

                case nameof(AssignmentsPage):
                    NavigateTo<SegmentPage>();
                    break;

                case nameof(AssignmentViewerPage):
                    NavigateTo<AssignmentsPage>();
                    break;
            }
        }
    }
}
