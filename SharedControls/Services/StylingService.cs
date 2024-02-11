using System.Windows;
using System.Windows.Media;

namespace Shared.Services
{
    public class StylingService
    {
        public Style CircularButtonStyle => (Style)Application.Current.Resources["CircularButton"];
        public Brush StagedBrush => (Brush)Application.Current.Resources["StagedBrush"];
        public Brush DeleteBrush => (Brush)Application.Current.Resources["DeleteBrush"];
        public Brush ReadyBrush => (Brush)Application.Current.Resources["ReadyBrush"];
        public Brush NeutralBrush => (Brush)Application.Current.Resources["NeutralBrush"];

    }
}
