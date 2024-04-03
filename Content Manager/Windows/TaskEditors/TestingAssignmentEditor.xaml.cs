using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Schema;

namespace Content_Manager.Windows.Editors
{
    public partial class TestingAssignmentEditor : Window, ITaskEditor
    {
        private TestingAssignment _taskAssignment;
        public IAssignment Assignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public TestingAssignmentEditor(TestingAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new TestingAssignment()
            {
                Title = txtTitle.Text
            };
            txtTitle.Text = _taskAssignment.Title;
            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var question in _taskAssignment.Questions)
            {
                var existingQuestionControl = new QuestionEditControl(question);
                existingQuestionControl.Discarded += OnQuestionDiscarded;

                spItems.Children.Add(existingQuestionControl);
            }

            var newQuestionControl = new QuestionEditControl();
            newQuestionControl.Committed += OnQuestionCommitted;
            spItems.Children.Add(newQuestionControl);

            //ContentStore.ItemUpdated += Question_Updated;
        }

        private void OnQuestionCommitted(Question question)
        {
            _taskAssignment.Questions.Add(question);
            RedrawUi();
        }

        private void OnQuestionDiscarded(Question question)
        {
            _taskAssignment.Questions.Remove(question);
            RedrawUi();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
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

