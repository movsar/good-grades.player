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
    public partial class ProverbBuilderTab : UserControl, IQuizTabControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ProverbBuilderTab()
        {
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }
        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var quizItem in _contentStore.SelectedSegment!.ProverbBuilderQuiz.QuizItems)
            {
                var existingQuizItem = new QuizItemControl(QuizTypes.ProverbBuilder, quizItem);
                existingQuizItem.Update += QuizItem_Save;
                existingQuizItem.Delete += QuizItem_Delete;

                spItems.Children.Add(existingQuizItem);
            }

            var newQuizItem = new QuizItemControl(QuizTypes.ProverbBuilder);
            newQuizItem.Create += QuizItem_Create;
            spItems.Children.Add(newQuizItem);
        }

        private void QuizItem_Create(IModelBase model)
        {
            _contentStore.SelectedSegment?.ProverbBuilderQuiz.QuizItems.Add(model as QuizItem);
         
            UpdateQuiz();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.ProverbBuilderQuiz == null)
            {
                return;
            }
            //var previewWindow = new ProverbSelectionQuizViewer(_contentStore.SelectedSegment.ProverbSelectionQuiz);
            //previewWindow.ShowDialog();
        }

        #region Event Handlers
        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.ProverbBuilder);
            RedrawUi();
        }

        private void QuizItem_Delete(string itemId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.ProverbBuilderQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            _contentStore.SelectedSegment?.ProverbBuilderQuiz.QuizItems.Remove(itemToRemove!);

            UpdateQuiz();
        }

        private void QuizItem_Save(string? id, IModelBase model)
        {
            UpdateQuiz();
        }


        #endregion
    }
}
