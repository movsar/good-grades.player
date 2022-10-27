using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls.MaterialControls;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows.Controls;

namespace Content_Manager.UserControls.QuizTabs
{
    public partial class TestingTab : UserControl, IQuizTabControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public TestingTab()
        {
            InitializeComponent();
            DataContext = this;

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();

            foreach (var question in _contentStore.SelectedSegment!.TestingQuiz.Questions)
            {
                var questionControl = new QuestionControl(question);
                questionControl.Update += QuestionControl_Save;
                questionControl.Delete += QuestionControl_Delete;

                spItems.Children.Add(questionControl);
            }

            var newQuestion = new QuestionControl();
            newQuestion.Create += Question_Create;

            spItems.Children.Add(newQuestion);
        }
        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.Testing);
            RedrawUi();
        }

        private void Question_Create(IModelBase model)
        {
            _contentStore.SelectedSegment?.TestingQuiz.Questions.Add(model as TestingQuestion);

            UpdateQuiz();
        }
        private void QuestionControl_Delete(string questionId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.TestingQuiz.Questions.Where(qi => qi.Id == questionId).First();
            _contentStore.SelectedSegment?.TestingQuiz.Questions.Remove(itemToRemove!);

            UpdateQuiz();
        }
        private void QuestionControl_Save(string? id, IModelBase model)
        {
            UpdateQuiz();
        }
    }
}
