using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows.Editors;
using Data;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Realms;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls.MaterialControls
{
    public partial class TaskMaterialControl : UserControl
    {
        #region Properties and Fields
        private FormCompletionInfo _formCompletionInfo;
        private TaskType _taskType;

        ContentStore _contentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService _stylingService => App.AppHost!.Services.GetRequiredService<StylingService>();

        public bool IsContentSet { get; set; }
        private string TaskId { get; }

        #endregion
        #region Initialization
        private void SetUiForNewMaterial()
        {            
            btnPreview.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
        }
        private void SetUiForExistingMaterial()
        {
            btnSetData.Background = _stylingService.ReadyBrush;

            if (IsContentSet)
            {
                btnSetData.Background = _stylingService.ReadyBrush;
            }

            btnPreview.Background = _stylingService.ReadyBrush;
            btnDelete.Visibility = Visibility.Visible;
        }

        private void SharedInitialization(bool isExistingMaterial = false)
        {
            InitializeComponent();
            DataContext = this;

            var propertiesToWatch = new List<string>(){
                nameof(IsContentSet)
            };

            _formCompletionInfo = new FormCompletionInfo(propertiesToWatch, isExistingMaterial);
            _formCompletionInfo.StatusChanged += OnFormStatusChanged;
        }

        public TaskMaterialControl()
        {
            SharedInitialization();
            AddTaskTypeOptions();
            SetUiForNewMaterial();
        }

        public TaskMaterialControl(ITaskMaterial material)
        {
            SharedInitialization(true);

            TaskId = material.Id!;

            SetUiForExistingMaterial();
        }
        #endregion
        private void OnFormStatusChanged(bool isReady)
        {
            if (isReady)
            {
                btnSave.Visibility = Visibility.Visible;
                btnPreview.Visibility = Visibility.Visible;
            }
            else
            {
                btnSave.Visibility = Visibility.Collapsed;
                btnPreview.Visibility = Visibility.Collapsed;
            }
        }

        private void AddTaskTypeOptions()
        {
            var fillingTaskType = new ComboBoxItem()
            {
                Content = Constants.TASK_NAME_FILLING
            };

            var selectingTaskType = new ComboBoxItem()
            {
                Content = Constants.TASK_NAME_SELECTING
            };

            var testingTaskType = new ComboBoxItem()
            {
                Content = Constants.TASK_NAME_TEST
            };

            var matchingTaskType = new ComboBoxItem()
            {
                Content = Constants.TASK_NAME_MATCHING
            };

            var buildingTaskMaterial = new ComboBoxItem()
            {
                Content = Constants.TASK_NAME_BUILDING
            };

            cmbTaskType.Items.Add(fillingTaskType);
            cmbTaskType.Items.Add(selectingTaskType);
            cmbTaskType.Items.Add(testingTaskType);
            cmbTaskType.Items.Add(matchingTaskType);
            cmbTaskType.Items.Add(buildingTaskMaterial);
        }


        private void btnSetData_Click(object sender, RoutedEventArgs e)
        {
            if (_taskType == TaskType.Matching)
            {
                var matchingTaskEditor = new MatchingTaskEditor();
                matchingTaskEditor.ShowDialog();
            }

            btnSetData.Background = _stylingService.StagedBrush;

            _formCompletionInfo.Update(nameof(IsContentSet), IsContentSet);
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            //if (_contentStore?.SelectedSegment?.CelebrityWordsQuiz == null)
            //{
            //    return;
            //}
            //var previewWindow = new CelebrityQuizPresenter(_contentStore.SelectedSegment.CelebrityWordsQuiz);
            //previewWindow.ShowDialog();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var rm = _contentStore.Database.Find<ITaskMaterial>(TaskId);
            _contentStore.Database.Write(() => _contentStore.Database.Remove(rm));
            _contentStore.RaiseItemDeletedEvent(rm);
        }

        private void cmbTaskType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmbItem = e.AddedItems[0] as ComboBoxItem;
            var selectedItemTitle = cmbItem?.Content.ToString();
            if (string.IsNullOrEmpty(selectedItemTitle))
            {
                return;
            }

            btnSetData.IsEnabled = true;
            _taskType = GetSelectedTaskType(selectedItemTitle);
        }

        private TaskType GetSelectedTaskType(string selectedTaskTypeTitle)
        {
            if (selectedTaskTypeTitle.Equals(Constants.TASK_NAME_FILLING))
            {
                return TaskType.Filling;
            }
            else if (selectedTaskTypeTitle.Equals(Constants.TASK_NAME_TEST))
            {
                return TaskType.Test;
            }
            else if (selectedTaskTypeTitle.Equals(Constants.TASK_NAME_BUILDING))
            {
                return TaskType.Building;
            }
            else if (selectedTaskTypeTitle.Equals(Constants.TASK_NAME_SELECTING))
            {
                return TaskType.Selecting;
            }
            else if (selectedTaskTypeTitle.Equals(Constants.TASK_NAME_MATCHING))
            {
                return TaskType.Matching;
            }
            else
            {
                throw new Exception("No matching TaskType has been found");
            }
        }
    }
}
