using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Content_Manager.Stores;
using Content_Manager.Interfaces;
using Data.Interfaces;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class ReadingTab : UserControl, ISegmentTabControl
    {
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ReadingTab()
        {
            InitializeComponent();
            DataContext = this;

            ContentStore.ItemAdded += ContentStore_ItemChanged;
            ContentStore.ItemDeleted += ContentStore_ItemChanged;
            ContentStore.ItemUpdated += ContentStore_ItemChanged;

            RedrawUi();
        }

        private void ContentStore_ItemChanged(IEntityBase entity)
        {
            RedrawUi();
        }

        public void RedrawUi()
        {
            spReadingMaterialControls.Children.Clear();

            if (ContentStore.SelectedSegment!.ReadingMaterials == null) return;

            foreach (var material in ContentStore.SelectedSegment!.ReadingMaterials)
            {
                var existingReadingMaterial = new ReadingMaterialControl(material);

                spReadingMaterialControls.Children.Add(existingReadingMaterial);
            }

            var rmcNew = new ReadingMaterialControl();

            spReadingMaterialControls.Children.Add(rmcNew);
        }

    }
}
