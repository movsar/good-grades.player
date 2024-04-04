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
    public partial class MatchingAssignmentEditor : Window, IAssignmentEditor
    {
        private MatchingAssignment _assignment;
        public IAssignment Assignment => _assignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public MatchingAssignmentEditor(MatchingAssignment? matchingTaskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = matchingTaskEntity ?? new MatchingAssignment()
            {
                Title = txtTitle.Text
            };
            txtTitle.Text = _assignment.Title;

            RedrawItems();
        }

        public void RedrawItems()
        {
            spItems.Children.Clear();
            foreach (var item in _assignment.Items)
            {
                var existingItemControl = new AssignmentItemEditControl(AssignmentType.Matching, item);
                existingItemControl.Discarded += OnAssignmentItemDiscarded;

                spItems.Children.Add(existingItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(AssignmentType.Matching);
            newItemControl.Committed += OnAssignmentItemCommitted;

            spItems.Children.Add(newItemControl);
        }

        private void OnAssignmentItemCommitted(AssignmentItem item)
        {
            _assignment.Items.Add(item);
            RedrawItems();
        }
        private void OnAssignmentItemDiscarded(AssignmentItem item)
        {
            _assignment.Items.Remove(item);
            RedrawItems();
        }

        private void SaveAndClose()
        {
            if (_assignment == null || string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите заголовок");
                return;
            }

            // Update assignment data
            _assignment.Title = txtTitle.Text;

            // Extract Assignment Items from UI
            _assignment.Items.Clear();
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

                if (!aiEditControl.IsValid)
                {
                    continue;
                }

                _assignment.Items.Add(aiEditControl.Item);
            }

            var existingAssignment = ContentStore.SelectedSegment!.MatchingAssignments.FirstOrDefault(a => a.Id == _assignment.Id);
            if (existingAssignment == null)
            {
                ContentStore.SelectedSegment!.MatchingAssignments.Add(_assignment);
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
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveAndClose();
        }

    }
}
