using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Controls;
using Shared.Viewers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls.QuizTabs
{
    public partial class CelebrityWordsTab : UserControl, IQuizTabControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public CelebrityWordsTab()
        {
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.CelebrityWodsQuiz == null)
            {
                return;
            }
            var previewWindow = new CelebrityQuizPresenter(_contentStore.SelectedSegment.CelebrityWodsQuiz);
            previewWindow.ShowDialog();
        }


        #region Event Handlers
        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.CelebrityWords);
            RedrawUi();
        }

        private void QuizItem_Delete(string itemId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.CelebrityWodsQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            _contentStore.SelectedSegment?.CelebrityWodsQuiz.QuizItems.Remove(itemToRemove!);

            UpdateQuiz();
        }

        private void QuizItem_Save(string? id, IModelBase model)
        {
            UpdateQuiz();
        }


        #endregion
        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var quizItem in _contentStore.SelectedSegment!.CelebrityWodsQuiz.QuizItems)
            {
                var existingQuizItemControl = new QuizItemControl(QuizTypes.CelebrityWords, quizItem);
                existingQuizItemControl.Update += QuizItem_Save;
                existingQuizItemControl.Delete += QuizItem_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newQuizItemControl = new QuizItemControl(QuizTypes.CelebrityWords);
            newQuizItemControl.Create += QuizItem_Create;
            spItems.Children.Add(newQuizItemControl);
        }

        private void QuizItem_Create(IModelBase model)
        {
            _contentStore.SelectedSegment?.CelebrityWodsQuiz.QuizItems.Add(model as QuizItem);
            UpdateQuiz();
        }
    }
}
