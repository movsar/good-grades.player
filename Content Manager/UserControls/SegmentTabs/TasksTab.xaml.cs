using Content_Manager.Stores;
using Content_Manager.UserControls.MaterialControls;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class TasksTab : UserControl
    {
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public TasksTab()
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
            spTaskMaterialControls.Children.Clear();

            foreach (var material in ContentStore.SelectedSegment!.MatchingTasks)
            {
                spTaskMaterialControls.Children.Add(new TaskMaterialControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.FillingTasks)
            {
                spTaskMaterialControls.Children.Add(new TaskMaterialControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.BuildingTasks)
            {
                spTaskMaterialControls.Children.Add(new TaskMaterialControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.TestingTasks)
            {
                spTaskMaterialControls.Children.Add(new TaskMaterialControl(material));
            }
            foreach (var material in ContentStore.SelectedSegment!.SelectingTasks)
            {
                spTaskMaterialControls.Children.Add(new TaskMaterialControl(material));
            }

            var newMaterial = new TaskMaterialControl();

            spTaskMaterialControls.Children.Add(newMaterial);
        }

        private void ContentStore_ItemChanged(IEntityBase entity)
        {
            RedrawUi();
        }
    }
}
