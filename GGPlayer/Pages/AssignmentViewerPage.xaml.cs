using Data.Entities;
using Data.Interfaces;
using GGPlayer.Services;
using Shared.Controls;
using Shared.Controls.Assignments;
using Shared.Interfaces;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Data.Entities.TaskItems;
using System.Text;
using Serilog.Filters;
using Microsoft.Extensions.DependencyInjection;

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

        public void LoadAssignment(IAssignment assignment)
        {
            _currentStep = 1;
            _assignment = assignment;
            _isAssignmentCompleted = false;

            switch (_assignment)
            {
                case MatchingAssignment:
                    _userControl = App.AppHost!.Services.GetRequiredService<MatchingAssignmentControl>();
                    _userControl.Initialize((MatchingAssignment)_assignment);
                    break;
                case TestingAssignment:
                    _userControl = App.AppHost!.Services.GetRequiredService<TestingAssignmentControl>();
                    _userControl.Initialize((TestingAssignment)_assignment);
                    break;
                case FillingAssignment:
                    _userControl = App.AppHost!.Services.GetRequiredService<FillingAssignmentControl>();
                    _userControl.Initialize((FillingAssignment)_assignment);
                    break;
                case SelectingAssignment:
                    _userControl = App.AppHost!.Services.GetRequiredService<SelectingAssignmentControl>();
                    _userControl.Initialize((SelectingAssignment)_assignment);
                    break;
                case BuildingAssignment:
                    _userControl = App.AppHost!.Services.GetRequiredService<BuildingAssignmentControl>();
                    _userControl.Initialize((BuildingAssignment)_assignment);
                    break;
            }

            ucRoot.Content = _userControl;

            _userControl.AssignmentItemSubmitted += _userControl_AssignmentItemSubmitted;
            _userControl.AssignmentCompleted += _userControl_AssignmentCompleted;

            SetUiStateToReady();
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

            if (_isAssignmentCompleted)
            {
                AssignmentCompleted?.Invoke(_assignment, true);
            }
            else
            {
                _userControl.OnNextClicked();
                SetUiStateToReady();
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

        private string GetAssignmentTitle()
        {
            if (_assignment is not TestingAssignment)
            {
                return _assignment.Title;
            }
            else
            {
                var testingAssignment = _assignment as TestingAssignment;
                var sb = new StringBuilder();
                sb.Append(_currentStep);
                sb.Append(" / ");
                sb.Append(testingAssignment.Questions.Count);
                sb.Append(" ");
                sb.Append(testingAssignment.Questions[_currentStep - 1].Text);
                return sb.ToString();
            }
        }

        #region UI states
        private void SetUiStateToReady()
        {
            tbTitle.Text = GetAssignmentTitle();

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