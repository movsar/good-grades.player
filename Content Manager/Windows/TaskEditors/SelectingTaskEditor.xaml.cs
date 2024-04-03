using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Content_Manager.Windows.Editors
{
    public partial class SelectingTaskEditor : Window, ITaskEditor
    {
        private SelectingAssignment _taskAssignment;
        public IAssignment TaskAssignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public SelectingTaskEditor(SelectingAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new SelectingAssignment()
            {
                Title = txtTitle.Text
            };
            txtTitle.Text = _taskAssignment.Question.Text;

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in _taskAssignment.Question.Options)
            {
                var existingQuizItemControl = new AssignmentItemEditControl(TaskType.Selecting, item);
                existingQuizItemControl.Delete += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Selecting);
            spItems.Children.Add(newItemControl);
        }

        private void ExistingQuizItemControl_SetAsCorrect(string itemId)
        {
            _taskAssignment.Question.CorrectOptionId = itemId;
            ContentStore.DbContext.SaveChanges();

            RedrawUi();
        }

        private void Item_Create(IEntityBase entity)
        {
            var itemEntity = (AssignmentItem)entity;

            // Add the Task entity
            var taskState = ContentStore.DbContext.Entry(_taskAssignment).State;
            if (taskState == EntityState.Detached || taskState == EntityState.Added)
            {
                ContentStore.SelectedSegment!.SelectingTasks.Add(_taskAssignment);
            }

            // Add the Task item entity
            _taskAssignment.Question.Options.Add(itemEntity);
            ContentStore.DbContext.SaveChanges();

            RedrawUi();
        }

        private void Item_Delete(string id)
        {
            var itemToRemove = _taskAssignment.Question.Options.First(i => i.Id == id);
            _taskAssignment.Question.Options.Remove(itemToRemove);
            ContentStore.DbContext.SaveChanges();

            RedrawUi();
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_taskAssignment != null)
            {
                _taskAssignment.Question.Text = txtTitle.Text;
                ContentStore.DbContext.SaveChanges();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _taskAssignment.Question.Options.Clear();

            foreach (var item in spItems.Children)
            {
                var aiEditControl = item as AssignmentItemEditControl;
                if (aiEditControl == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(aiEditControl.Item.Text))
                {
                    continue;
                }

                _taskAssignment.Question.Options.Add(aiEditControl.Item);
            }

            ContentStore.DbContext.ChangeTracker.DetectChanges();
            ContentStore.DbContext.SaveChanges();
        }
    }
}
