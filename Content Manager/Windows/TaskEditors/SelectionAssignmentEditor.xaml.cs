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

namespace Content_Manager.Windows.Editors
{
    public partial class SelectionAssignmentEditor : Window, ITaskEditor
    {
        private SelectingAssignment _assignment;
        public IAssignment Assignment => _assignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public SelectionAssignmentEditor(SelectingAssignment? assignment = null)
        {
            InitializeComponent();
            DataContext = this;

            if (assignment == null)
            {
                _assignment = new SelectingAssignment();
            }
            else
            {
                _assignment = assignment;
                txtTitle.Text = _assignment.Question.Text;
            }

            RedrawAssignmentItems();
        }

        public void RedrawAssignmentItems()
        {
            spItems.Children.Clear();
            foreach (var item in _assignment.Question.Options)
            {
                var existingQuizItemControl = new AssignmentItemEditControl(TaskType.Selecting, item);
                existingQuizItemControl.Discarded += OnAssignmentItemDiscarded;
                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Selecting);
            newItemControl.Committed += OnAssignmentItemCommitted;
            spItems.Children.Add(newItemControl);
        }

        private void SaveAssignment()
        {
            if (_assignment == null || string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите заголовок");
                return;
            }

            // Update assignment data
            _assignment.Title = txtTitle.Text;
            _assignment.Question.Text = txtTitle.Text;
            _assignment.Question.Options.Clear();

            // Extract Assignment Items from UI
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

                _assignment.Question.Options.Add(aiEditControl.Item);
            }

            if (_assignment.Question.Options.Count < 2 || _assignment.Question.Options.FirstOrDefault(o => o.IsChecked == true) == null)
            {
                MessageBox.Show("Добавьте хотя бы два варианта ответа и хотя бы один выберите как правильный");
                return;
            }

            // If it's a new task, add it to the selected segment
            var existingAssignment = ContentStore.SelectedSegment!.SelectionAssignments.FirstOrDefault(st => st.Id == _assignment.Id);
            if (existingAssignment == null)
            {
                ContentStore.SelectedSegment!.SelectionAssignments.Add(_assignment);
            }

            // Save and notify the changes
            ContentStore.DbContext.ChangeTracker.DetectChanges();
            ContentStore.DbContext.SaveChanges();

            if (existingAssignment == null)
            {
                ContentStore.RaiseItemAddedEvent(_assignment);
            }
            else
            {
                ContentStore.RaiseItemUpdatedEvent(_assignment);
            }

            Close();
        }

        #region Event Handlers
        private void OnAssignmentItemDiscarded(AssignmentItem item)
        {
            _assignment.Question.Options.Remove(item);
            RedrawAssignmentItems();
        }

        private void OnAssignmentItemCommitted(AssignmentItem item)
        {
            _assignment.Question.Options.Add(item);
            RedrawAssignmentItems();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveAssignment();
        }
        #endregion
    }
}
