using Content_Manager.Stores;
using Data.Enums;
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

namespace Content_Manager.UserControls.Tabs
{
    public partial class GapFillerQuizTab : UserControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public GapFillerQuizTab()
        {
            InitializeComponent();
            DataContext = this;

            foreach (var quizItem in _contentStore.SelectedSegment!.GapFillerQuiz.QuizItems)
            {
                var existingQuizItem = new QuizItemControl(QuizTypes.GapFiller, quizItem);
                existingQuizItem.Add += QuizItem_Add;
                existingQuizItem.Save += QuizItem_Save;
                existingQuizItem.Delete += QuizItem_Delete;

                spItems.Children.Add(existingQuizItem);
            }

            var newQuizItem = new QuizItemControl(QuizTypes.GapFiller);
            newQuizItem.Add += QuizItem_Add;
            newQuizItem.Save += QuizItem_Save;
            newQuizItem.Delete += QuizItem_Delete;
            spItems.Children.Add(newQuizItem);
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.GapFillerQuiz == null)
            {
                return;
            }
            //var previewWindow = new ProverbSelectionQuizViewer(_contentStore.SelectedSegment.ProverbSelectionQuiz);
            //previewWindow.ShowDialog();
        }

        #region Event Handlers
        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.GapFiller);
        }

        private void QuizItem_Delete(string itemId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.GapFillerQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            _contentStore.SelectedSegment?.GapFillerQuiz.QuizItems.Remove(itemToRemove!);

            UpdateQuiz();
        }

        private void QuizItem_Save()
        {
            UpdateQuiz();
        }

        private void QuizItem_Add(QuizItem quizItem)
        {
            _contentStore.SelectedSegment?.GapFillerQuiz.QuizItems.Add(quizItem);

            UpdateQuiz();
        }
        #endregion
    }
}
