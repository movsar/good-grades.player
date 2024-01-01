using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows.Editors;
using Data;
using Data.Entities.Materials;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls.MaterialControls
{
    public partial class TaskMaterialControl : UserControl
    {
        #region Properties and Fields
        private FormCompletionInfo _formCompletionInfo;
        private TaskType _taskType;
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();

        public bool IsContentSet { get; set; }

        private readonly ITaskMaterial _taskMaterial;

        #endregion

        #region Initialization
        private void SetUiForNewMaterial()
        {
            btnPreview.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
        }
        private void SetUiForExistingMaterial()
        {
            btnSetData.Background = StylingService.ReadyBrush;

            if (IsContentSet)
            {
                btnSetData.Background = StylingService.ReadyBrush;
            }

            btnPreview.Background = StylingService.ReadyBrush;
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
            AddTaskTypes();
            SetUiForNewMaterial();
        }

        public TaskMaterialControl(ITaskMaterial taskMaterial)
        {
            SharedInitialization(true);
            AddTaskTypes();
            SetSelectedTaskType(taskMaterial);

            _taskMaterial = taskMaterial;

            SetUiForExistingMaterial();
        }
        #endregion
        private void OnFormStatusChanged(bool isReady)
        {
            if (isReady)
            {
                btnPreview.Visibility = Visibility.Visible;
            }
            else
            {
                btnPreview.Visibility = Visibility.Collapsed;
            }
        }
        private void btnSetData_Click(object sender, RoutedEventArgs e)
        {
            if (_taskType == TaskType.Matching)
            {
                var matchingTaskEditor = new MatchingTaskEditor(_taskMaterial as MatchingTaskEntity);
                matchingTaskEditor.ShowDialog();
            }

            btnSetData.Background = StylingService.StagedBrush;

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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ContentStore.Database.Write(() =>
            {
                ContentStore.Database.Remove(_taskMaterial);
            });

            ContentStore.RaiseItemDeletedEvent(_taskMaterial);
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
        private void AddTaskTypes()
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
        private void SetSelectedTaskType(ITaskMaterial taskMaterial)
        {
            string selectedTaskName = taskMaterial switch
            {
                FillingTaskEntity _ => Constants.TASK_NAME_FILLING,
                SelectingTaskEntity _ => Constants.TASK_NAME_SELECTING,
                TestingTaskEntity _ => Constants.TASK_NAME_TEST,
                BuildingTaskEntity _ => Constants.TASK_NAME_BUILDING,
                MatchingTaskEntity _ => Constants.TASK_NAME_MATCHING,
                _ => ""
            };

            cmbTaskType.SelectedItem = cmbTaskType.Items
                                .Cast<ComboBoxItem>()
                                .FirstOrDefault(item => item.Content.ToString() == selectedTaskName);
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
