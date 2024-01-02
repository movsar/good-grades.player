using Content_Manager.Interfaces;
using Content_Manager.Models;
using Content_Manager.Services;
using Content_Manager.Stores;
using Content_Manager.Windows.Editors;
using Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls
{
    public partial class TaskAssignmentControl : UserControl
    {
        #region Properties and Fields
        private FormCompletionInfo _formCompletionInfo;
        private TaskType _taskType;
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        StylingService StylingService => App.AppHost!.Services.GetRequiredService<StylingService>();

        public bool IsContentSet { get; set; }

        private readonly ITaskAssignment _taskMaterial;

        #endregion

        #region Initialization
        private void SetUiForNewMaterial()
        {
            btnPreview.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
        }
        private void SetUiForExistingMaterial()
        {
            btnSetData.Background = IsContentSet ? StylingService.ReadyBrush : StylingService.StagedBrush;
            btnPreview.Background = StylingService.ReadyBrush;
            btnDelete.Visibility = Visibility.Visible;
            cmbTaskType.IsEnabled = false;
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

        public TaskAssignmentControl()
        {
            SharedInitialization();

            AddTaskTypes();
            SetUiForNewMaterial();
        }

        public TaskAssignmentControl(ITaskAssignment taskMaterial)
        {
            _taskMaterial = taskMaterial;
            IsContentSet = GetCurrentTaskItems().Count() > 0;

            SharedInitialization(true);

            AddTaskTypes();
            SetSelectedTaskType();

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
            ITaskAssignment taskAssignment;
            ITaskEditor taskEditor = _taskType switch
            {
                TaskType.Matching => new MatchingTaskEditor(_taskMaterial as MatchingTaskAssignment),
                TaskType.Filling => new FillingTaskEditor(_taskMaterial as FillingTaskAssignment),
                TaskType.Selecting => new SelectingTaskEditor(_taskMaterial as SelectingTaskAssignment),
                TaskType.Building => new BuildingTaskEditor(_taskMaterial as BuildingTaskAssignment),
                TaskType.Test => new TestingTaskEditor(_taskMaterial as TestingTaskAssignment),
                _ => throw new NotImplementedException()
            };

            taskEditor.ShowDialog();
            taskAssignment = taskEditor.TaskAssignment;

            if (_taskMaterial != null)
            {
                // Data update may or may not have been done
                ContentStore.RaiseItemUpdatedEvent(taskAssignment);
            }
            else if (taskAssignment.IsContentSet)
            {
                // Content has been specified for a new task
                ContentStore.RaiseItemAddedEvent(taskAssignment);
            }
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
        private IEnumerable<object> GetCurrentTaskItems()
        {
            var items = _taskMaterial switch
            {
                MatchingTaskAssignment mt => mt.Items,
                FillingTaskAssignment ft => ft.Items,
                //SelectingTaskAssignment st => st.,
                //TestingTaskAssignment _ => Constants.TASK_NAME_TEST,
                //BuildingTaskAssignment _ => Constants.TASK_NAME_BUILDING,
                //_ => ""
            };

            return items;
        }
        private void SetSelectedTaskType()
        {
            string selectedTaskName = _taskMaterial switch
            {
                FillingTaskAssignment _ => Constants.TASK_NAME_FILLING,
                SelectingTaskAssignment _ => Constants.TASK_NAME_SELECTING,
                TestingTaskAssignment _ => Constants.TASK_NAME_TEST,
                BuildingTaskAssignment _ => Constants.TASK_NAME_BUILDING,
                MatchingTaskAssignment _ => Constants.TASK_NAME_MATCHING,
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
