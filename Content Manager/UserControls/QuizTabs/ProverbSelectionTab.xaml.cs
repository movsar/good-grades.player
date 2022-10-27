using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Shared.Viewers;
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
    public partial class ProverbSelectionTab : UserControl, IQuizTabControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ProverbSelectionTab()
        {
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.ProverbSelectionQuiz == null)
            {
                return;
            }
            //var previewWindow = new ProverbSelectionQuizViewer(_contentStore.SelectedSegment.ProverbSelectionQuiz);
            //previewWindow.ShowDialog();
        }

        private void QuizItem_SetAsCorrect(string itemId)
        {
            _contentStore.SelectedSegment!.ProverbSelectionQuiz!.CorrectQuizId = itemId;
            UpdateQuiz();
        }

        #region Event Handlers
        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.ProverbSelection);
            RedrawUi();
        }

        private void QuizItem_Delete(string itemId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.ProverbSelectionQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            _contentStore.SelectedSegment?.ProverbSelectionQuiz.QuizItems.Remove(itemToRemove!);

            UpdateQuiz();
        }

        private void QuizItem_Save(string? id, IModelBase model)
        {
            if (id == null)
            {
                _contentStore.SelectedSegment?.ProverbSelectionQuiz.QuizItems.Add(model as QuizItem);
            }
            UpdateQuiz();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var quizItem in _contentStore.SelectedSegment!.ProverbSelectionQuiz.QuizItems)
            {
                var isSelected = _contentStore.SelectedSegment!.ProverbSelectionQuiz.CorrectQuizId == quizItem.Id;

                var existingQuizItem = new QuizItemControl(QuizTypes.ProverbSelection, quizItem, isSelected);
                existingQuizItem.Update += QuizItem_Save;
                existingQuizItem.Delete += QuizItem_Delete;
                existingQuizItem.SetAsCorrect += QuizItem_SetAsCorrect;

                spItems.Children.Add(existingQuizItem);
            }

            var newQuizItem = new QuizItemControl(QuizTypes.ProverbSelection);
            newQuizItem.Update += QuizItem_Save;
            newQuizItem.Delete += QuizItem_Delete;

            spItems.Children.Add(newQuizItem);
        }

        #endregion

    }
}
