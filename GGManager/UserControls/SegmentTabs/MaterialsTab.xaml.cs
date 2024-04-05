using GGManager.Stores;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace GGManager.UserControls.SegmentTabs
{
    public partial class MaterialsTab : UserControl
    {
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public MaterialsTab()
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
            spListeningControls.Children.Clear();

            if (ContentStore.SelectedSegment == null)
            {
                return;
            }

            foreach (var material in ContentStore.SelectedSegment!.Materials)
            {
                var existingMaterial = new MaterialControl(material);
                spListeningControls.Children.Add(existingMaterial);
            }

            var newMaterial = new MaterialControl();

            spListeningControls.Children.Add(newMaterial);
        }

        private void ContentStore_ItemChanged(IEntityBase entity)
        {
            RedrawUi();
        }
    }
}
