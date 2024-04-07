using GGManager.Interfaces;
using GGManager.Stores;
using GGManager.UserControls;
using Data;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Translations;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GGManager.Windows.Editors
{
    public partial class FillingAssignmentEditor : Window, IAssignmentEditor
    {
        private FillingAssignment _assignment;
        public IAssignment Assignment => _assignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public FillingAssignmentEditor(FillingAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = taskEntity ?? new FillingAssignment()
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
                var existingItemControl = new AssignmentItemEditControl(AssignmentType.Filling, item);
                existingItemControl.Discarded += OnAssignmentItemDiscarded;

                spItems.Children.Add(existingItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(AssignmentType.Filling);
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
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите заголовок");
                return;
            }

            if (_assignment.Items.Count == 0)
            {
                MessageBox.Show("Нужно добавить элементы");
                return;
            }

            // Update assignment data
            _assignment.Title = txtTitle.Text;
            IAssignmentEditor.SetAssignmentItems(_assignment.Items, spItems);

            var existingAssignment = ContentStore.SelectedSegment!.FillingAssignments.FirstOrDefault(a => a.Id == _assignment.Id);
            if (existingAssignment == null)
            {
                ContentStore.SelectedSegment!.FillingAssignments.Add(_assignment);
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
