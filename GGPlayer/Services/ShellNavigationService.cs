using System.Windows.Controls;

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

        public bool CanGoBack => _frame?.CanGoBack ?? false;

        public void GoBack()
        {
            if (CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
