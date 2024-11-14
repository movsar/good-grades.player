using Data.Entities;
using Data.Interfaces;
using GGPlayer.Services;
using Shared.Controls;
using Shared.Controls.Assignments;
using Shared.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

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

        public void Initialize(IAssignment assignment)
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

            SetUiStateToReady();
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

            SetUiStateToReady();
            _userControl.OnRetryClicked();
        }
        private void btnCheck_MouseUp(object sender, MouseButtonEventArgs e) => _userControl.OnCheckClicked();
        private void btnNext_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentStep++;

            SetUiStateToReady();

            if (_isAssignmentCompleted)
            {
                AssignmentCompleted?.Invoke(_assignment, true);
            }
            else
            {
                _userControl.OnNextClicked();
            }
        }
        private void btnPrevious_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _currentStep--;

            SetUiStateToReady();

            _userControl.OnPreviousClicked();
        }

        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (btnCheck.Visibility == Visibility.Visible)
                {
                    btnCheck_MouseUp(this, null);
                }
                else if (btnNext.Visibility == Visibility.Visible)
                {
                    btnNext_MouseUp(this, null);
                }
            }
        }

        #region UI states
        private void SetUiStateToReady()
        {
            btnRetry.Visibility = Visibility.Collapsed;

            if (_currentStep >= _userControl.StepsCount)
            {
                btnCheck.Visibility = Visibility.Visible;
                btnNext.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnCheck.Visibility = Visibility.Collapsed;
                btnNext.Visibility = Visibility.Visible;
            }

            btnPrevious.Visibility = (_currentStep > 1) ? Visibility.Visible : Visibility.Collapsed;

            msgFailure.Visibility = Visibility.Collapsed;
            msgSuccess.Visibility = Visibility.Collapsed;
        }
        private void SetUiStateToFailure()
        {
            btnNext.Visibility = Visibility.Collapsed;

            btnRetry.Visibility = Visibility.Visible;
            btnCheck.Visibility = Visibility.Collapsed;

            msgFailure.Visibility = Visibility.Visible;
            msgSuccess.Visibility = Visibility.Collapsed;
        }
        private void SetUiStateToSuccess()
        {
            btnNext.Visibility = Visibility.Visible;
            btnCheck.Visibility = Visibility.Collapsed;

            btnRetry.Visibility = Visibility.Collapsed;

            msgFailure.Visibility = Visibility.Collapsed;
            msgSuccess.Visibility = Visibility.Visible;
        }
        #endregion
    }
}