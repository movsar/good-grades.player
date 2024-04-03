using Content_Manager.Models;
using Content_Manager.Services;
using Data;
using Data.Entities.TaskItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Shared.Translations;
using Image = System.Windows.Controls.Image;

namespace Content_Manager.UserControls
{
    public partial class AssignmentItemEditControl : UserControl
    {
        #region Fields
        private string Hint = Ru.SetDescription;
        private FormCompletionInfo _formCompletionInfo;
        private TaskType _taskType;
        #endregion

        #region Properties and Events
        public AssignmentItem Item { get; }
        public event Action<AssignmentItem> Discarded;
        public event Action<AssignmentItem> Committed;
        #endregion

        #region Reactions
        private void OnTextSet(bool isSet)
        {
            _formCompletionInfo.Update(nameof(Item.Text), isSet);
        }
        private void OnImageSet(bool isSet = true)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.StreamSource = new MemoryStream(Item.Image!);
            logo.EndInit();

            var imgControl = new Image();
            imgControl.VerticalAlignment = VerticalAlignment.Stretch;
            imgControl.Source = logo;
            btnChooseImage.Content = imgControl;

            _formCompletionInfo.Update(nameof(Item.Image), isSet);
        }
        #endregion

        #region Initialization

        private void SharedUiInitialization(TaskType taskType, bool isExistingItem)
        {
            InitializeComponent();
            DataContext = this;

            _taskType = taskType;

            var propertiesToWatch = new List<string>
            {
                nameof(Item.Text)
            };

            // Decide what controls to make available
            switch (_taskType)
            {
                case TaskType.Filling:
                    Hint = Ru.FillingQuestion;
                    break;
                case TaskType.Matching:
                    Hint = Ru.SetImageDescription;

                    btnChooseImage.Visibility = Visibility.Visible;

                    propertiesToWatch.Add(nameof(Item.Image));

                    break;
                case TaskType.Test:
                case TaskType.Selecting:
                    chkIsChecked.Visibility = Visibility.Visible;
                    chkIsChecked.IsChecked = Item.IsChecked;
                    break;
                default:
                    break;
            }

            txtItemText.Text = Hint;

            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingItem);
        }
        public AssignmentItemEditControl(TaskType taskType)
        {
            Item = new AssignmentItem();

            // Prepare UI for a new Assignment Item
            SharedUiInitialization(taskType, false);
            btnCommit.Visibility = Visibility.Visible;
            btnDiscard.Visibility = Visibility.Collapsed;
        }
        public AssignmentItemEditControl(TaskType taskType, AssignmentItem item)
        {
            Item = item;

            // Prepare UI for an existing Assignment Item
            SharedUiInitialization(taskType, true);
            btnDiscard.Visibility = Visibility.Visible;
            btnCommit.Visibility = Visibility.Collapsed;

            txtItemText.Text = Item.Text;
            OnTextSet(true);

            if (Item.Image != null)
            {
                OnImageSet(true);
            }
        }
        #endregion

        #region Event Handlers
        private void txtItemText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtItemText.Text == Hint)
            {
                txtItemText.Text = "";
            }
        }

        private void txtItemText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemText.Text))
            {
                txtItemText.Text = Hint;
            }
        }

        private void txtItemText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_formCompletionInfo == null)
            {
                // Not initialized yet
                return;
            }

            if (string.IsNullOrEmpty(txtItemText.Text) || txtItemText.Text.Equals(Hint))
            {
                OnTextSet(false);
            }
            else
            {
                Item.Text = txtItemText.Text;
                OnTextSet(true);
            }
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectImageFilePath();
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllBytes(filePath);
            if (content.Length == 0) return;

            Item.Image = content;

            OnImageSet(true);
        }

        private void btnDiscard_Click(object sender, RoutedEventArgs e)
        {
            Discarded?.Invoke(Item);
        }

        private void chkIsChecked_Checked(object sender, RoutedEventArgs e)
        {
            Item.IsChecked = true;
        }

        private void chkIsChecked_Unchecked(object sender, RoutedEventArgs e)
        {
            Item.IsChecked = false;
        }

        private void btnCommit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Item.Text))
            {
                return;
            }

            btnCommit.Visibility = Visibility.Collapsed;
            btnDiscard.Visibility = Visibility.Visible;

            Committed?.Invoke(Item);
        }
        #endregion
    }
}
