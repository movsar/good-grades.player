using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            newQuizItem.Create += QuizItem_Create; ;

            spItems.Children.Add(newQuizItem);
        }

        private void QuizItem_Create(IModelBase model)
        {
            _contentStore.CreateSelectableQuizItem(QuizTypes.ProverbSelection, model as QuizItem, _contentStore.SelectedSegment?.ProverbSelectionQuiz!);
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
            _contentStore.DeleteSelectableQuizItem(QuizTypes.ProverbSelection, itemId, _contentStore.SelectedSegment!.ProverbSelectionQuiz);
            RedrawUi();
        }

        private void QuizItem_Save(string? id, IModelBase model)
        {
            UpdateQuiz();
        }
        #endregion

    }
}
