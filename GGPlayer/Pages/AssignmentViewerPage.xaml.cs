using Data.Entities;
using Data.Interfaces;
using GGManager.UserControls;
using Serilog.Parsing;
using Shared.Controls.Assignments;
using Shared.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;

namespace GGPlayer.Pages.Assignments
{
    public partial class AssignmentViewerPage : Page
    {
        public event Action<IAssignment, bool> AssignmentCompleted;

        private IAssignmentViewer _userControl;
        private bool _isAssignmentCompleted;
        private readonly IAssignment _assignment;

        public AssignmentViewerPage(ShellWindow _shell, IAssignment assignment)
        {
            InitializeComponent();

            _assignment = assignment;
            SetUiStateToInitial();
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
            _userControl.AssignmentItemSubmitted += _userControl_AssignmentItemSubmitted;
            _userControl.AssignmentCompleted += _userControl_AssignmentCompleted;
        }

        private void _userControl_AssignmentCompleted(IAssignment assignment, bool success)
        {
            if (!success)
            {
                SetUiStateToFailure();
            }
            else
            {
                SetUiStateToSuccess();
            }

            _isAssignmentCompleted = success;
        }

        private void _userControl_AssignmentItemSubmitted(IAssignment assignment, string itemId, bool success)
        {
            if (!success)
            {
                SetUiStateToFailure();
            }
            else
            {
                SetUiStateToSuccess();
            }
        }

        private void btnRetry_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetUiStateToInitial();
            _userControl.OnRetryClicked();
        }

        private void btnCheck_MouseUp(object sender, MouseButtonEventArgs e) => _userControl.OnCheckClicked();

        ~AssignmentViewerPage()
        {
            _userControl.AssignmentCompleted -= _userControl_AssignmentCompleted;
            _userControl.AssignmentItemSubmitted -= _userControl_AssignmentItemSubmitted;
        }

        private void btnNextAssignment_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SetUiStateToInitial();

            if (_isAssignmentCompleted)
            {
                AssignmentCompleted?.Invoke(_assignment, true);
            }
            else
            {
                _userControl.OnNextClicked();
            }
        }

        #region Buttons and success / failure messages' states
        private void SetUiStateToInitial()
        {
            btnNextAssignment.Visibility = System.Windows.Visibility.Collapsed;

            btnRetry.Visibility = System.Windows.Visibility.Collapsed;
            btnCheck.Visibility = System.Windows.Visibility.Visible;

            msgFailure.Visibility = System.Windows.Visibility.Collapsed;
            msgSuccess.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void SetUiStateToFailure()
        {
            btnRetry.Visibility = System.Windows.Visibility.Visible;
            btnCheck.Visibility = System.Windows.Visibility.Collapsed;

            msgFailure.Visibility = System.Windows.Visibility.Visible;
            msgSuccess.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void SetUiStateToSuccess()
        {
            btnNextAssignment.Visibility = System.Windows.Visibility.Visible;

            btnRetry.Visibility = System.Windows.Visibility.Collapsed;
            btnCheck.Visibility = System.Windows.Visibility.Collapsed;

            msgFailure.Visibility = System.Windows.Visibility.Collapsed;
            msgSuccess.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion
    }
}