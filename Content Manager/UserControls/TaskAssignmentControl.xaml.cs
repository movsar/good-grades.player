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
using Shared.Viewers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ru = Shared.Translations.Ru;

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

        private readonly ITaskAssignment _taskAssignment;

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
            _taskAssignment = taskMaterial;
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
                TaskType.Matching => new MatchingTaskEditor(_taskAssignment as MatchingTaskAssignment),
                TaskType.Filling => new FillingTaskEditor(_taskAssignment as FillingTaskAssignment),
                TaskType.Selecting => new SelectingTaskEditor(_taskAssignment as SelectingTaskAssignment),
                TaskType.Building => new BuildingTaskEditor(_taskAssignment as BuildingTaskAssignment),
                TaskType.Test => new TestingTaskEditor(_taskAssignment as TestingTaskAssignment),
                _ => throw new NotImplementedException()
            };

            taskEditor.ShowDialog();
            taskAssignment = taskEditor.TaskAssignment;

            if (_taskAssignment != null)
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
            Window viewer = null!;
            switch (_taskType)
            {
                case TaskType.Matching:
                    viewer = new MatchingViewer((MatchingTaskAssignment)_taskAssignment);
                    break;

                case TaskType.Test:
                    viewer = new TestingViewer((TestingTaskAssignment)_taskAssignment);
                    break;

                case TaskType.Filling:
                    viewer = new FillingViewer((FillingTaskAssignment)_taskAssignment);
                    break;

                case TaskType.Selecting:
                    break;
                case TaskType.Building:
                    break;
            }

            viewer.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ContentStore.Database.Write(() =>
            {
                ContentStore.Database.Remove(_taskAssignment);
            });

            ContentStore.RaiseItemDeletedEvent(_taskAssignment);
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
                Content = Ru.FillingTaskName
            };

            var selectingTaskType = new ComboBoxItem()
            {
                Content = Ru.SelectingTaskName
            };

            var testingTaskType = new ComboBoxItem()
            {
                Content = Ru.TestTaskName
            };

            var matchingTaskType = new ComboBoxItem()
            {
                Content = Ru.MatchingTaskName
            };

            var buildingTaskMaterial = new ComboBoxItem()
            {
                Content = Ru.BuildingTaskName
            };

            cmbTaskType.Items.Add(fillingTaskType);
            cmbTaskType.Items.Add(selectingTaskType);
            cmbTaskType.Items.Add(testingTaskType);
            cmbTaskType.Items.Add(matchingTaskType);
            cmbTaskType.Items.Add(buildingTaskMaterial);
        }
        private IEnumerable<object> GetCurrentTaskItems()
        {
            IEnumerable<object> items = _taskAssignment switch
            {
                MatchingTaskAssignment mt => mt.Items,
                FillingTaskAssignment ft => ft.Items,
                SelectingTaskAssignment st => st.Items,
                TestingTaskAssignment tt => tt.Questions,
                BuildingTaskAssignment bt => bt.Items,
                _ => throw new NotImplementedException()
            };

            return items;
        }
        private void SetSelectedTaskType()
        {
            string selectedTaskName = _taskAssignment switch
            {
                FillingTaskAssignment _ => Ru.FillingTaskName,
                SelectingTaskAssignment _ => Ru.SelectingTaskName,
                TestingTaskAssignment _ => Ru.TestTaskName,
                BuildingTaskAssignment _ => Ru.BuildingTaskName,
                MatchingTaskAssignment _ => Ru.MatchingTaskName,
                _ => ""
            };

            cmbTaskType.SelectedItem = cmbTaskType.Items
                                .Cast<ComboBoxItem>()
                                .FirstOrDefault(item => item.Content.ToString() == selectedTaskName);
        }
        private TaskType GetSelectedTaskType(string selectedTaskTypeTitle)
        {
            if (selectedTaskTypeTitle.Equals(Ru.FillingTaskName))
            {
                return TaskType.Filling;
            }
            else if (selectedTaskTypeTitle.Equals(Ru.TestTaskName))
            {
                return TaskType.Test;
            }
            else if (selectedTaskTypeTitle.Equals(Ru.BuildingTaskName))
            {
                return TaskType.Building;
            }
            else if (selectedTaskTypeTitle.Equals(Ru.SelectingTaskName))
            {
                return TaskType.Selecting;
            }
            else if (selectedTaskTypeTitle.Equals(Ru.MatchingTaskName))
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
