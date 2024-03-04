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
    public partial class SelectingTaskEditor : Window, ITaskEditor
    {
        private SelectingTaskAssignment _taskAssignment;
        public IAssignment TaskAssignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public SelectingTaskEditor(SelectingTaskAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new SelectingTaskAssignment()
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
                var isSelected = _taskAssignment.Question.CorrectOptionId == item.Id;
                var existingQuizItemControl = new AssignmentItemEditControl(TaskType.Selecting, item, isSelected);
                existingQuizItemControl.Delete += Item_Delete;
                existingQuizItemControl.SetAsCorrect += ExistingQuizItemControl_SetAsCorrect;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Selecting);
            newItemControl.Create += Item_Create;
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
    }
}
