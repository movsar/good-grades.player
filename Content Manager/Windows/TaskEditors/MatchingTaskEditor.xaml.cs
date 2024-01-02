using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;

namespace Content_Manager.Windows.Editors
{
    /// <summary>
    /// Interaction logic for TextToImageQuizEditor.xaml
    /// </summary>
    public partial class MatchingTaskEditor : Window, ITaskEditor
    {

        private readonly ContentStore ContentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        private MatchingTaskAssignment _taskAssignment;
        public ITaskAssignment TaskAssignment => _taskAssignment;


        public MatchingTaskEditor(MatchingTaskAssignment? matchingTaskEntity = null)
        {
            _taskAssignment = matchingTaskEntity ?? new MatchingTaskAssignment()
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
            foreach (var item in _taskAssignment.Items)
            {
                var existingQuizItemControl = new ItemControl(item);
                existingQuizItemControl.Update += Item_Update;
                existingQuizItemControl.Delete += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new ItemControl();
            newItemControl.Create += Item_Create;
            newItemControl.Update += Item_Update;
            spItems.Children.Add(newItemControl);
        }

        private void Item_Delete(string id)
        {
            ContentStore.Database.Write(() =>
            {
                var itemToRemove = _taskAssignment.Items.First(i => i.Id == id);
                _taskAssignment.Items.Remove(itemToRemove);
            });

            RedrawUi();
        }

        private void Item_Update(IEntityBase entity)
        {
            // does this even need to have any code?
        }

        private void Item_Create(IEntityBase entity)
        {

            var e = (TextAndImageItem)entity;

            if (!_taskAssignment.IsManaged)
            {
                ContentStore.Database.Write(() => ContentStore.SelectedSegment!.MatchingTasks.Add(_taskAssignment));
            }

            ContentStore.Database.Write(() => _taskAssignment.Items.Add(e));
            RedrawUi();
        }
    }
}
