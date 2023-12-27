using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Content_Manager.Stores;
using Content_Manager.Interfaces;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class ReadingTab : UserControl, ISegmentTabControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ReadingTab()
        {
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }

        public void RedrawUi()
        {
            spReadingMaterialControls.Children.Clear();

            if (_contentStore.SelectedSegment!.ReadingMaterials == null) return;

            foreach (var material in _contentStore.SelectedSegment!.ReadingMaterials)
            {
                var existingReadingMaterial = new ReadingMaterialControl(material);
                //existingReadingMaterial.Update += ReadingMaterialControl_Save;
                //existingReadingMaterial.Delete += ReadingMaterialControl_Delete;

                spReadingMaterialControls.Children.Add(existingReadingMaterial);
            }

            var rmcNew = new ReadingMaterialControl();
            //rmcNew.Create += RmcNew_Create;

            spReadingMaterialControls.Children.Add(rmcNew);
        }

    }
}
