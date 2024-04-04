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
    public partial class BuildingAssignmentEditor : Window, IAssignmentEditor
    {
        private BuildingAssignment _assignment;
        public IAssignment Assignment => _assignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public BuildingAssignmentEditor(BuildingAssignment? assignment = null)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = assignment ?? new BuildingAssignment()
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
                var existingItemControl = new AssignmentItemEditControl(AssignmentType.Building, item);
                existingItemControl.Discarded += OnAssignmentItemDiscarded;

                spItems.Children.Add(existingItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(AssignmentType.Building);
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

                _assignment.Items.Add(aiEditControl.Item);
            }

            var existingAssignment = ContentStore.SelectedSegment!.BuildingAssignments.FirstOrDefault(a => a.Id == _assignment.Id);
            if (existingAssignment == null)
            {
                ContentStore.SelectedSegment!.BuildingAssignments.Add(_assignment);
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
