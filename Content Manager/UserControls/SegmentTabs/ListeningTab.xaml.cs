using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class ListeningTab : UserControl, ISegmentTabControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ListeningTab()
        {
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }
        public void RedrawUi()
        {
            spListeningMaterialControls.Children.Clear();

            if (_contentStore.SelectedSegment!.ListeningMaterials == null) return;

            foreach (var material in _contentStore.SelectedSegment!.ListeningMaterials)
            {
                var existingMaterial = new ListeningMaterialControl(material);
                //existingMaterial.Update += ExistingMaterial_Save;
                //existingMaterial.Delete += ExistingMaterial_Delete;
                spListeningMaterialControls.Children.Add(existingMaterial);
            }

            var newMaterial = new ListeningMaterialControl();
            //newMaterial.Create += NewMaterial_Create; 

            spListeningMaterialControls.Children.Add(newMaterial);
        }

        private void NewMaterial_Create(IEntityBase entity)
        {
            //_contentStore.SaveCurrentSegment();
            RedrawUi();
        }

        private void ExistingMaterial_Delete(string id)
        {
            //_contentStore.DeleteListeningMaterial(id);
            RedrawUi();
        }

        private void ExistingMaterial_Save(string? arg1, IEntityBase arg2)
        {
            //_contentStore.SaveCurrentSegment();
            RedrawUi();
        }
    }
}
