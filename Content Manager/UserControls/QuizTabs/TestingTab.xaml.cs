using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls.MaterialControls;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                questionControl.Save += QuestionControl_Save;
                questionControl.Delete += QuestionControl_Delete;

                spItems.Children.Add(questionControl);

            }
            var newQuestion = new QuestionControl();
            spItems.Children.Add(newQuestion);
            newQuestion.Save += QuestionControl_Save;
            newQuestion.Delete += QuestionControl_Delete;
        }

        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.Testing);
            RedrawUi();
        }

        private void QuestionControl_Delete(string questionId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.TestingQuiz.Questions.Where(qi => qi.Id == questionId).First();
            _contentStore.SelectedSegment?.TestingQuiz.Questions.Remove(itemToRemove!);

            UpdateQuiz();
        }

        private void QuestionControl_Save(string? id, IModelBase model)
        {
            if (id == null)
            {
                _contentStore.SelectedSegment?.TestingQuiz.Questions.Add(model as TestingQuestion);
            }
            UpdateQuiz();
        }
    }
}
