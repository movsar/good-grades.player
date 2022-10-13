using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Content_Manager.Services {
    public class StylingService {
        public Brush StagedBrush => (Brush)Application.Current.Resources["StagedBrush"];
        public Brush DeleteBrush => (Brush)Application.Current.Resources["DeleteBrush"];
        public Brush ReadyBrush => (Brush)Application.Current.Resources["ReadyBrush"];

    }
}
