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
        private Frame _frame;

        public void Initialize(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(Page page)
        {
            _frame.Navigate(page);
        }
        public void NavigateTo<T>() where T : Page
        {
            var page = App.AppHost!.Services.GetRequiredService<T>();
            _frame.Navigate(page);
        }

        public bool CanGoBack => _frame?.CanGoBack ?? false;

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
