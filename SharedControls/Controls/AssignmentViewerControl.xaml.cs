using Data.Entities;
using Data.Interfaces;
using Shared.Controls.Assignments;
using Shared.Interfaces;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shared.Controls
{
    public partial class AssignmentViewerControl : UserControl
    {
        public event Action<IAssignment, bool> AssignmentCompleted;

        private IAssignmentControl _userControl;
        private IAssignment _assignment;

        private bool _isAssignmentCompleted;
        private int _currentStep;

        public AssignmentViewerControl()
        {
            InitializeComponent();
        }

        public void Initialize(IAssignment assignment, IAssignmentControl viewer)
        {
            _userControl = viewer;
            _currentStep = 1;
            _assignment = assignment;
            _isAssignmentCompleted = false;

            switch (_assignment)
            {
                case MatchingAssignment:
                    _userControl.Initialize((MatchingAssignment)_assignment);
                    break;
                case TestingAssignment:
                    _userControl.Initialize((TestingAssignment)_assignment);
                    break;
                case FillingAssignment:
                    _userControl.Initialize((FillingAssignment)_assignment);
                    break;
                case SelectingAssignment:
                    _userControl.Initialize((SelectingAssignment)_assignment);
                    break;
                case BuildingAssignment:
                    _userControl.Initialize((BuildingAssignment)_assignment);
                    break;
            }

            ucRoot.Content = _userControl;

            _userControl.AssignmentItemSubmitted -= _userControl_AssignmentItemSubmitted;
            _userControl.AssignmentItemSubmitted += _userControl_AssignmentItemSubmitted;
            _userControl.AssignmentCompleted -= _userControl_AssignmentCompleted;
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
            btnPrevious.Visibility = (_currentStep > 1) ? Visibility.Visible : Visibility.Collapsed;

            btnCheck.Visibility = Visibility.Collapsed;
            btnRetry.Visibility = Visibility.Visible;

            msgFailure.Visibility = Visibility.Visible;
            msgSuccess.Visibility = Visibility.Collapsed;
        }
        private void SetUiStateToSuccess()
        {
            btnNext.Visibility = Visibility.Visible;
            btnPrevious.Visibility = Visibility.Collapsed;

            btnCheck.Visibility = Visibility.Collapsed;
            btnRetry.Visibility = Visibility.Collapsed;

            msgFailure.Visibility = Visibility.Collapsed;
            msgSuccess.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
