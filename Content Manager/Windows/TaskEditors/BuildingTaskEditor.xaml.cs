using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.Windows.Editors
{
    public partial class BuildingTaskEditor : Window, ITaskEditor
    {
        private BuildingTaskAssignment _taskAssignment;
        public IAssignment TaskAssignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public BuildingTaskEditor(BuildingTaskAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new BuildingTaskAssignment()
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
                existingQuizItemControl.Delete += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Building);
            newItemControl.Create += Item_Create;
            spItems.Children.Add(newItemControl);
        }

        private void Item_Create(IEntityBase entity)
        {
            var itemEntity = (AssignmentItem)entity;

            ContentStore.Database.Write(() =>
            {
                // Add the Task entity
                if (_taskAssignment.IsManaged == false)
                {
                    ContentStore.SelectedSegment!.BuildingTasks.Add(_taskAssignment);
                }

                // Add the Task item entity
                _taskAssignment.Items.Add(itemEntity);
            });

            RedrawUi();
        }

        private void Item_Delete(string id)
        {
            ContentStore.Database.Write(() =>
            {
                var itemToRemove = _taskAssignment.Items.First(i => i.Id == id);
                _taskAssignment.Items.Remove(itemToRemove);
            });

            RedrawUi();
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_taskAssignment != null)
            {
                ContentStore.Database.Write(() => _taskAssignment.Title = txtTitle.Text);
            }
        }
    }
}
