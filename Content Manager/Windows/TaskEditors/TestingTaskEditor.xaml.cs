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
    public partial class TestingTaskEditor : Window, ITaskEditor
    {
        private TestingTaskAssignment _taskAssignment;
        public ITaskAssignment TaskAssignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public TestingTaskEditor(TestingTaskAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new TestingTaskAssignment()
            {
                Title = txtTitle.Text
            };

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in _taskAssignment.Questions)
            {
                var existingQuizItemControl = new TestingQuestionControl(item);
                existingQuizItemControl.Delete += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new TestingQuestionControl();
            newItemControl.Create += Item_Create;
            spItems.Children.Add(newItemControl);
        }

        private void Item_Create(IEntityBase entity)
        {
            var itemEntity = (TestingQuestion)entity;

            ContentStore.Database.Write(() =>
            {
                // Add the Task entity
                if (_taskAssignment.IsManaged == false)
                {
                    ContentStore.SelectedSegment!.TestingTasks.Add(_taskAssignment);
                }

                // Add the Task item entity
                _taskAssignment.Questions.Add(itemEntity);
            });

            RedrawUi();
        }

        private void Item_Delete(string id)
        {
            ContentStore.Database.Write(() =>
            {
                var itemToRemove = _taskAssignment.Questions.First(i => i.Id == id);
                _taskAssignment.Questions.Remove(itemToRemove);
            });

            RedrawUi();
        }
    }
}
