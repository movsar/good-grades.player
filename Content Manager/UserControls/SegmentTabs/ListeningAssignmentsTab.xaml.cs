using Content_Manager.Stores;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class ListeningAssignmentsTab : UserControl
    {
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ListeningAssignmentsTab()
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

            foreach (var material in ContentStore.SelectedSegment!.ListeningMaterials)
            {
                var existingMaterial = new ListeningAssignmentControl(material);
                spListeningControls.Children.Add(existingMaterial);
            }

            var newMaterial = new ListeningAssignmentControl();

            spListeningControls.Children.Add(newMaterial);
        }

        private void ContentStore_ItemChanged(IEntityBase entity)
        {
            RedrawUi();
        }
    }
}
