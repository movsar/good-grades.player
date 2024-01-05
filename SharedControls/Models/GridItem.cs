using System.Windows;

namespace Shared.Models
{
    public class GridItem
    {
        public FrameworkElement Element { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
