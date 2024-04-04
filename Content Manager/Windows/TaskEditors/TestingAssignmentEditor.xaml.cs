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
    public partial class TestingAssignmentEditor : Window, ITaskEditor
    {
        private TestingAssignment _assignment;
        public IAssignment Assignment => _assignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public TestingAssignmentEditor(TestingAssignment? assignment = null)
        {
            InitializeComponent();
            DataContext = this;

            _assignment = assignment ?? new TestingAssignment()
            {
                Title = txtTitle.Text
            };

            txtTitle.Text = _assignment.Title;

            RedrawQuestions();
        }

        public void RedrawQuestions()
        {
            spItems.Children.Clear();
            foreach (var question in _assignment.Questions)
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
            _assignment.Questions.Add(question);
            RedrawQuestions();
        }

        private void OnQuestionDiscarded(Question question)
        {
            _assignment.Questions.Remove(question);
            RedrawQuestions();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (_assignment == null || !_assignment.Questions.Any() || string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Убедитесь что вы заполнили заголовок и добавили вопросы!");
                return;
            }

            // Update assignment data
            _assignment.Title = txtTitle.Text;

            // Extract Questions from UI
            _assignment.Questions.Clear();
            foreach (var item in spItems.Children)
            {
                var questionEditControl = item as QuestionEditControl;
                if (questionEditControl == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(questionEditControl.Question.Text))
                {
                    continue;
                }

                _assignment.Questions.Add(questionEditControl.Question);
            }

            // If it's a new task, add it to the selected segment
            var existingAssignment = ContentStore.SelectedSegment!.TestingTasks.FirstOrDefault(st => st.Id == _assignment.Id);
            if (existingAssignment == null)
            {
                ContentStore.SelectedSegment!.TestingTasks.Add(_assignment);
            }

            // Save and notify the changes
            ContentStore.DbContext.ChangeTracker.DetectChanges();
            ContentStore.DbContext.SaveChanges();

            if (existingAssignment == null)
            {
                ContentStore.RaiseItemAddedEvent(_assignment);
            }
            else
            {
                ContentStore.RaiseItemUpdatedEvent(_assignment);
            }

            Close();
        }
    }
}

