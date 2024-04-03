using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.Windows.Editors
{
    public partial class BuildingTaskEditor : Window, ITaskEditor
    {
        private BuildingAssignment _taskAssignment;
        public IAssignment Assignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public BuildingTaskEditor(BuildingAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new BuildingAssignment()
            {
                Title = txtTitle.Text
            };
            txtTitle.Text = _taskAssignment.Title;

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in _taskAssignment.Items)
            {
                var existingQuizItemControl = new AssignmentItemEditControl(TaskType.Building, item);
                //existingQuizItemControl.Removed += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Building);
            spItems.Children.Add(newItemControl);
        }

        private void Item_Create(IEntityBase entity)
        {
            var itemEntity = (AssignmentItem)entity;

            // Add the Task entity
            var taskState = ContentStore.DbContext.Entry(_taskAssignment).State;
            if (taskState == EntityState.Detached || taskState == EntityState.Added)
            {
                ContentStore.SelectedSegment!.BuildingTasks.Add(_taskAssignment);
            }

            // Add the Task item entity
            _taskAssignment.Items.Add(itemEntity);

            ContentStore.DbContext.SaveChanges();
            RedrawUi();
        }

        private void Item_Delete(string id)
        {
            var itemToRemove = _taskAssignment.Items.First(i => i.Id == id);
            _taskAssignment.Items.Remove(itemToRemove);
            ContentStore.DbContext.SaveChanges();

            RedrawUi();
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_taskAssignment != null)
            {
                _taskAssignment.Title = txtTitle.Text;
                ContentStore.DbContext.SaveChanges();
            }
        }
    }
}
