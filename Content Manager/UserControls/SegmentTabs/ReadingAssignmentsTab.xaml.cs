using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Content_Manager.Stores;
using Data.Interfaces;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class ReadingAssignmentsTab : UserControl
    {
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ReadingAssignmentsTab()
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
            spReadingAssignmentControls.Children.Clear();

            if (ContentStore.SelectedSegment == null)
            {
                return;
            }

            foreach (var material in ContentStore.SelectedSegment!.ReadingMaterials)
            {
                var existingReadingMaterial = new ReadingAssignmentControl(material);

                spReadingAssignmentControls.Children.Add(existingReadingMaterial);
            }

            var rmcNew = new ReadingAssignmentControl();

            spReadingAssignmentControls.Children.Add(rmcNew);
        }

    }
}
