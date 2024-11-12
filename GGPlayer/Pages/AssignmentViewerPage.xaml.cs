using Data.Entities;
using Data.Interfaces;
using GGPlayer.Services;
using Shared.Controls;
using Shared.Controls.Assignments;
using Shared.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GGPlayer.Pages.Assignments
{
    public partial class AssignmentViewerPage : Page
    {

        public event Action<IAssignment, bool> AssignmentCompleted;

        private readonly ShellNavigationService _navigationService;

        private IAssignmentViewer _userControl;
        private IAssignment _assignment;

        private bool _isAssignmentCompleted;
        private int _currentStep;
        public AssignmentViewerPage(ShellNavigationService navigationService)
        {
            InitializeComponent();

            _navigationService = navigationService;
        }

        public void LoadAssignmentView(IAssignment assignment)
        {
            _currentStep = 1;
            _assignment = assignment;
            _isAssignmentCompleted = false;

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

            ucRoot.Content = uc;

            _userControl = (IAssignmentViewer)uc;
            _userControl.AssignmentItemSubmitted += _userControl_AssignmentItemSubmitted;
            _userControl.AssignmentCompleted += _userControl_AssignmentCompleted;

            SetUiStateToInitial();
        }

        private void _userControl_AssignmentCompleted(IAssignment assignment, bool success)
        {
            if (assignment is TestingAssignment)
            {
                var testingAssignmentControl = _userControl as TestingAssignmentControl;
                ucRoot.Content = new StatisticsControl(testingAssignmentControl.CorrectAnswers, testingAssignmentControl.IncorrectAnswers);
            }

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
            _currentStep = 1;
            if (Content != _userControl)
            {
                ucRoot.Content = _userControl;
            }

            SetUiStateToInitial();
            _userControl.OnRetryClicked();
        }
        private void btnCheck_MouseUp(object sender, MouseButtonEventArgs e) => _userControl.OnCheckClicked();
        private void btnNext_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentStep++;

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
            btnRetry.Visibility = System.Windows.Visibility.Collapsed;

            if (_currentStep >= _userControl.StepsCount)
            {
                btnCheck.Visibility = System.Windows.Visibility.Visible;
                btnNext.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                btnCheck.Visibility = System.Windows.Visibility.Collapsed;
                btnNext.Visibility = System.Windows.Visibility.Visible;
            }

            msgFailure.Visibility = System.Windows.Visibility.Collapsed;
            msgSuccess.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void SetUiStateToFailure()
        {
            btnNext.Visibility = System.Windows.Visibility.Collapsed;

            btnRetry.Visibility = System.Windows.Visibility.Visible;
            btnCheck.Visibility = System.Windows.Visibility.Collapsed;

            msgFailure.Visibility = System.Windows.Visibility.Visible;
            msgSuccess.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void SetUiStateToSuccess()
        {
            btnNext.Visibility = System.Windows.Visibility.Visible;
            btnCheck.Visibility = System.Windows.Visibility.Collapsed;

            btnRetry.Visibility = System.Windows.Visibility.Collapsed;

            msgFailure.Visibility = System.Windows.Visibility.Collapsed;
            msgSuccess.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        private void btnPrevious_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_navigationService.CanGoBack)
            {
                _navigationService.GoBack();
            }
        }

        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (btnCheck.Visibility == System.Windows.Visibility.Visible)
                {
                    btnCheck_MouseUp(this, null);
                }
                else if (btnNext.Visibility == System.Windows.Visibility.Visible)
                {
                    btnNext_MouseUp(this, null);
                }
            }
        }
    }
}