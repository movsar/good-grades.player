using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;

namespace Content_Manager.Windows.Editors
{
    public partial class SelectingTaskEditor : Window, ITaskEditor
    {
        private SelectingTaskAssignment _taskAssignment;
        public ITaskAssignment TaskAssignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public SelectingTaskEditor(SelectingTaskAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new SelectingTaskAssignment()
            {
                Title = txtTitle.Text
            };

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in _taskAssignment.Items)
            {
                var existingQuizItemControl = new ItemControl(item);
                existingQuizItemControl.Delete += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new ItemControl();
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
                    ContentStore.SelectedSegment!.SelectingTasks.Add(_taskAssignment);
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
    }
}
