using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            txtTitle.Text = _taskAssignment.Title;

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in _taskAssignment.Questions)
            {
                var existingQuestionControl = new TestingQuestionControl(_taskAssignment, item);
                existingQuestionControl.QuestionDeleted += Question_Deleted;
                existingQuestionControl.QuestionUpdated += Question_Updated;

                spItems.Children.Add(existingQuestionControl);
            }

            var newItemControl = new TestingQuestionControl(_taskAssignment);
            newItemControl.QuestionCreated += Question_Updated;
            spItems.Children.Add(newItemControl);

            ContentStore.ItemUpdated += Question_Updated;
        }

        private void Question_Updated(IEntityBase quiestion)
        {
            RedrawUi();
        }

        private void Question_Deleted(string id)
        {
            ContentStore.Database.Write(() =>
            {
                var itemToRemove = _taskAssignment.Questions.First(i => i.Id == id);
                _taskAssignment.Questions.Remove(itemToRemove);
            });

            RedrawUi();
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_taskAssignment != null)
            {
                ContentStore.Database.Write(() => _taskAssignment.Title = txtTitle.Text);
            }
        }
    }
}
