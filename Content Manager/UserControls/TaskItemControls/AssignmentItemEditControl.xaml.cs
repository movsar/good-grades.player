using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Data;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using Shared.Translations;
using Shared.Services;
using System.Windows.Input;

namespace Content_Manager.UserControls
{
    public partial class AssignmentItemEditControl : UserControl
    {
        public event Action<string> Delete;

        #region Fields
        private string Hint = Ru.SetDescription;
        private FormCompletionInfo _formCompletionInfo;
        private TaskType _taskType;
        #endregion

        #region Properties
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();
        public AssignmentItem Item { get; }
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
            logo.StreamSource = new MemoryStream(Item.Image);
            logo.EndInit();

            var imgControl = new Image();
            imgControl.VerticalAlignment = VerticalAlignment.Stretch;
            imgControl.Source = logo;
            btnChooseImage.Content = imgControl;

            _formCompletionInfo.Update(nameof(Item.Image), isSet);
        }
        #endregion

        #region Initialization
        private void SetUiForNewMaterial()
        {
            btnDelete.Visibility = Visibility.Hidden;
        }
        private void SetUiForExistingMaterial()
        {
            btnDelete.Visibility = Visibility.Visible;
        }
        private void SharedUiInitialization(TaskType taskType, bool isExistingMaterial, bool isSelected)
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

            Item.Text = Hint;

            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingMaterial);
        }
        public AssignmentItemEditControl(TaskType taskType)
        {
            Item = new AssignmentItem();

            SharedUiInitialization(taskType, false, false);
            SetUiForNewMaterial();
        }
        public AssignmentItemEditControl(TaskType taskType, AssignmentItem item, bool isSelected = false)
        {
            Item = item;

            SharedUiInitialization(taskType, true, isSelected);
            SetUiForExistingMaterial();

            txtItemText.Text = Item.Text;
            OnTextSet(true);

            if (Item.Image != null)
            {
                OnImageSet(true);
            }
        }
        #endregion

        #region TextHandlers
        private void txtItemText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtItemText.Text == Hint)
            {
                Item.Text = "";
            }
        }

        private void txtItemText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemText.Text))
            {
                Item.Text = Hint;
            }
        }

        private void txtItemText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemText.Text) || txtItemText.Text.Equals(Hint))
            {
                OnTextSet(false);
            }
            else
            {
                OnTextSet(true);
            }
        }
        #endregion

        #region ButtonHandlers
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke(Item.Id);
        }
        #endregion

        private void chkIsChecked_Checked(object sender, RoutedEventArgs e)
        {
            Item.IsChecked = true;
        }

        private void chkIsChecked_Unchecked(object sender, RoutedEventArgs e)
        {
            Item.IsChecked = false;
        }
    }
}
