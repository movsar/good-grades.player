using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
using Data.Entities;
using Data.Entities.Materials;
using Data.Entities.Materials.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;

namespace Content_Manager.Windows.Editors
{
    /// <summary>
    /// Interaction logic for TextToImageQuizEditor.xaml
    /// </summary>
    public partial class MatchingTaskEditor : Window
    {

        private readonly ContentStore ContentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public MatchingTaskEntity MatchingTaskEntity { get; private set; }
        public MatchingTaskEditor(MatchingTaskEntity? matchingTaskEntity = null)
        {
            MatchingTaskEntity = matchingTaskEntity ?? new MatchingTaskEntity()
            {
                Title = "Adsad"
            };

            InitializeComponent();
            DataContext = this;

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in MatchingTaskEntity.Items)
            {
                var existingQuizItemControl = new TextAndImageItemControl(item);
                existingQuizItemControl.Update += Item_Update;
                existingQuizItemControl.Delete += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new TextAndImageItemControl();
            newItemControl.Create += Item_Create;
            newItemControl.Update += Item_Update;
            spItems.Children.Add(newItemControl);
        }

        private void Item_Delete(string id)
        {
            ContentStore.Database.Write(() =>
            {
                var itemToRemove = MatchingTaskEntity.Items.First(i => i.Id == id);
                MatchingTaskEntity.Items.Remove(itemToRemove);
            });    
            
            RedrawUi();
        }

        private void Item_Update(IEntityBase entity)
        {
            // does this even need to have any code?
        }

        private void Item_Create(IEntityBase entity)
        {

            var e = (TextAndImageItemEntity)entity;

            if (!MatchingTaskEntity.IsManaged)
            {
                ContentStore.Database.Write(() => ContentStore.SelectedSegment!.MatchingTasks.Add(MatchingTaskEntity));
            }

            ContentStore.Database.Write(() => MatchingTaskEntity.Items.Add(e));
            RedrawUi();
        }
    }
}
