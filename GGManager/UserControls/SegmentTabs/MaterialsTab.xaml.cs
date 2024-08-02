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
            //подписка на события изменений в contentStore
            ContentStore.ItemAdded += ContentStore_ItemChanged;
            ContentStore.ItemDeleted += ContentStore_ItemChanged;
            ContentStore.ItemUpdated += ContentStore_ItemChanged;

            RedrawUi();
        }
        //перерисовка интерфейса
        public void RedrawUi()
        {
            spListeningControls.Children.Clear();

            if (ContentStore.SelectedSegment == null)
            {
                return;
            }
            //добавление существующих материалов сегмента в интерфейс
            foreach (var material in ContentStore.SelectedSegment!.Materials)
            {
                var existingMaterial = new MaterialControl(material);
                spListeningControls.Children.Add(existingMaterial);
            }
            //добавление пустого поля для создания нового материала
            var newMaterial = new MaterialControl();

            spListeningControls.Children.Add(newMaterial);
        }

        private void ContentStore_ItemChanged(IEntityBase entity)
        {
            RedrawUi();
        }
    }
}
