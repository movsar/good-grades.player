using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Schema;

namespace Content_Manager.Windows.Editors
{
    public partial class TestingTaskEditor : Window, ITaskEditor
    {
        private TestingTaskAssignment _taskAssignment;
        public IAssignment TaskAssignment => _taskAssignment;
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
                var existingQuestionControl = new TestingQuestionEditControl(_taskAssignment, item);
                existingQuestionControl.QuestionDeleted += Question_Deleted;
                existingQuestionControl.QuestionUpdated += Question_Updated;

                spItems.Children.Add(existingQuestionControl);
            }

            var newItemControl = new TestingQuestionEditControl(_taskAssignment);
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
            var itemToRemove = _taskAssignment.Questions.First(i => i.Id == id);
            _taskAssignment.Questions.Remove(itemToRemove);
            ContentStore.DbContext.SaveChanges();

            RedrawUi();
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_taskAssignment != null)
            {
                _taskAssignment.Title = txtTitle.Text;
                ContentStore.DbContext.SaveChanges();
            }
        }

        //ошибка при неправильном заполнении, где item - созданный вариант с вопросом, CorrectOptionId-правильный вариант ответа, а Options.Count-количество ответов в вопросе
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var item in _taskAssignment.Questions)
            {
                if (string.IsNullOrEmpty(item.CorrectOptionId) == true || item.Options.Count < 2)
                {
                    MessageBox.Show("Вы неправильно заполнили данные! У вопроса должны быть минимум 2 варианта ответа и один правильный!");
                    e.Cancel = true;
                }
            }
        }
    }
}

