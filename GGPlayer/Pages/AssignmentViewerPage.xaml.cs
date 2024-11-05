using Data.Entities;
using Data.Interfaces;
using GGManager.UserControls;
using Shared.Controls.Assignments;
using Shared.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;

namespace GGPlayer.Pages.Assignments
{
    public partial class AssignmentViewerPage : Page
    {
        private IAssignmentViewer _userControl;
        private readonly IAssignment _assignment;

        public AssignmentViewerPage(ShellWindow _shell, IAssignment assignment)
        {
            InitializeComponent();

            _assignment = assignment;
            LoadAssignmentView();
        }

        private void LoadAssignmentView()
        {
            UserControl uc = null!;
            switch (_assignment)
            {
                case MatchingAssignment:
                    uc = new MatchingAssignmentControl((MatchingAssignment)_assignment);
                    break;
                case TestingAssignment:
                    uc = new TestingAssignmentControl((TestingAssignment)_assignment);
                    break;
                case FillingAssignment:
                    uc = new FillingAssignmentControl((FillingAssignment)_assignment);
                    break;
                case SelectingAssignment:
                    uc = new SelectionAssignmentControl((SelectingAssignment)_assignment);
                    break;
                case BuildingAssignment:
                    uc = new BuildingAssignmentControl((BuildingAssignment)_assignment);
                    break;
            }

            ucRoot.Children.Clear();
            ucRoot.Children.Add(uc);

            _userControl = (IAssignmentViewer)uc;
        }

        private void btnRetry_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoadAssignmentView();  
        }

        private void btnCheck_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_userControl.Check())
            {

            }
            else
            {

            }
        }
    }
}