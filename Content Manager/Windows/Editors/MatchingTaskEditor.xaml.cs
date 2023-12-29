using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
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
        public MatchingTaskEntity TaskEntity { get; private set; }
        public MatchingTaskEditor(MatchingTaskEntity? matchingTaskEntity = null)
        {
            TaskEntity = matchingTaskEntity ?? new MatchingTaskEntity();
            
            InitializeComponent();
            DataContext = this;

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in TaskEntity.Items)
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
            var itemToRemove = TaskEntity.Items.First(i => i.Id == id);
            TaskEntity.Items.Remove(itemToRemove);
        }

        private void Item_Update(IEntityBase entity)
        {
            // does this even need to have any code?
        }

        private void Item_Create(IEntityBase entity)
        {
            TaskEntity.Items.Add(entity as TextAndImageItemEntity);
            RedrawUi();
        }
    }
}
