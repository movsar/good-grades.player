using Content_Manager.Stores;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class TaskAssignmentsTab : UserControl
    {
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public TaskAssignmentsTab()
        {
            InitializeComponent();
            DataContext = this;

            ContentStore.ItemAdded += ContentStore_ItemChanged;
            ContentStore.ItemDeleted += ContentStore_ItemChanged;
            ContentStore.ItemUpdated += ContentStore_ItemChanged;

            RedrawUi();
        }
        public void RedrawUi()
        {
            spTaskAssignmentControls.Children.Clear();

            foreach (var material in ContentStore.SelectedSegment!.MatchingTasks)
            {
                spTaskAssignmentControls.Children.Add(new TaskAssignmentControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.FillingTasks)
            {
                spTaskAssignmentControls.Children.Add(new TaskAssignmentControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.BuildingTasks)
            {
                spTaskAssignmentControls.Children.Add(new TaskAssignmentControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.TestingTasks)
            {
                spTaskAssignmentControls.Children.Add(new TaskAssignmentControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.SelectingTasks)
            {
                spTaskAssignmentControls.Children.Add(new TaskAssignmentControl(material));
            }

            var newMaterial = new TaskAssignmentControl();

            spTaskAssignmentControls.Children.Add(newMaterial);
        }

        private void ContentStore_ItemChanged(IEntityBase entity)
        {
            RedrawUi();
        }
    }
}
