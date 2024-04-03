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
        private SelectingAssignment _taskAssignment;
        public IAssignment TaskAssignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public SelectingTaskEditor(SelectingAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            if (taskEntity == null)
            {
                _taskAssignment = new SelectingAssignment();
            }
            else
            {
                _taskAssignment = taskEntity;
                txtTitle.Text = _taskAssignment.Question.Text;
            }

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in _taskAssignment.Question.Options)
            {
                var existingQuizItemControl = new AssignmentItemEditControl(TaskType.Selecting, item);
                existingQuizItemControl.Committed += OnAssignmentItemCommitted;
                existingQuizItemControl.Discarded += OnAssignmentItemDiscarded;
                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Selecting);
            newItemControl.Committed += OnAssignmentItemCommitted;
            spItems.Children.Add(newItemControl);
        }

        private void OnAssignmentItemDiscarded(AssignmentItem item)
        {
            _taskAssignment.Question.Options.Remove(item);
            RedrawUi();
        }

        private void OnAssignmentItemCommitted(AssignmentItem item)
        {
            _taskAssignment.Question.Options.Add(item);
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

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Save()
        {
            if (_taskAssignment == null || string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                return;
            }

            _taskAssignment.Title = txtTitle.Text;
            _taskAssignment.Question.Text = txtTitle.Text;

            // If it's a new task, add it to the selected segment
            var existingTaskAssignment = ContentStore.SelectedSegment!.SelectingTasks.FirstOrDefault(st => st.Id == _taskAssignment.Id);
            if (existingTaskAssignment == null)
            {
                ContentStore.SelectedSegment!.SelectingTasks.Add(_taskAssignment);
            }

            // Update question with its options
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
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
    }
}
