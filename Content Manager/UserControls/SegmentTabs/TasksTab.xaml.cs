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

            if (ContentStore.SelectedSegment!.ListeningMaterials == null) return;

            foreach (var material in ContentStore.SelectedSegment!.ListeningMaterials)
            {
                //var existingMaterial = new TaskMaterialControl(material);
                //spTaskMaterialControls.Children.Add(existingMaterial);
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
