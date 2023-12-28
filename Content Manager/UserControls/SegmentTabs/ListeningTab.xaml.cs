using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class ListeningTab : UserControl, ISegmentTabControl
    {
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ListeningTab()
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
            spListeningMaterialControls.Children.Clear();

            if (ContentStore.SelectedSegment!.ListeningMaterials == null) return;

            foreach (var material in ContentStore.SelectedSegment!.ListeningMaterials)
            {
                var existingMaterial = new ListeningMaterialControl(material);
                spListeningMaterialControls.Children.Add(existingMaterial);
            }

            var newMaterial = new ListeningMaterialControl();

            spListeningMaterialControls.Children.Add(newMaterial);
        }

        private void ContentStore_ItemChanged(IEntityBase entity)
        {
            RedrawUi();
        }
    }
}
