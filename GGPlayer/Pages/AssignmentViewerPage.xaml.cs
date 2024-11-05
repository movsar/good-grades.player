using System.Windows.Controls;

namespace GGPlayer.Pages.Assignments
{
    public partial class AssignmentViewerPage : Page
    {
        public AssignmentViewerPage(ShellWindow _shell, UserControl assignmentControl)
        {
            InitializeComponent();

            ucRoot.Children.Add(assignmentControl);
        }
    }
}