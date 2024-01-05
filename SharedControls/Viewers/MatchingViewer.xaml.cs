using Data.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Shared.Viewers
{
    public partial class MatchingViewer : Window
    {
        public MatchingViewer(MatchingTaskAssignment assignment)
        {
            InitializeComponent();
        }

        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // This marks the start of the drag
            var border = sender as Border;
            if (border != null)
            {
                DragDrop.DoDragDrop(border, new DataObject(typeof(Border), border), DragDropEffects.Move);
            }
        }

        private void Element_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Element_Drop(object sender, DragEventArgs e)
        {
            var targetBorder = sender as Border;
            var sourceBorder = e.Data.GetData(typeof(Border)) as Border;

            if (sourceBorder == null || targetBorder == null || sourceBorder == targetBorder)
            {
                return;
            }

            // Get the contents of the Borders
            UIElement sourceContent = sourceBorder.Child;
            UIElement targetContent = targetBorder.Child;

            // Only allow elements of the same type to be swapped
            if (sourceContent.GetType() != targetContent.GetType())
            {
                return;
            }

            // Disconnect the children from their current parent Borders
            sourceBorder.Child = null;
            targetBorder.Child = null;

            // Swap the contents of the Borders
            sourceBorder.Child = targetContent;
            targetBorder.Child = sourceContent;
        }



    }
}