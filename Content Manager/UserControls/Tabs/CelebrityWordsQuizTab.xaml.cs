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
using static System.Net.Mime.MediaTypeNames;

namespace Content_Manager.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for CelebrityWordsQuizTab.xaml
    /// </summary>
    public partial class CelebrityWordsQuizTab : UserControl
    {
        private ContentStore _contentStore { get; }
        public CelebrityWordsQuizTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
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

        private void _contentStore_SegmentChanged(Segment selectedSegment)
        {
            if (selectedSegment == null) return;

            spItems.Children.Clear();

            foreach (var quizItem in selectedSegment.CelebrityWodsQuiz.QuizItems)
            {
                var existingQuizItem = new QuizItemControl(QuizTypes.CelebrityWords, quizItem);
                existingQuizItem.Add += QuizItem_Add;
                existingQuizItem.Save += QuizItem_Save;
                existingQuizItem.Delete += QuizItem_Delete;

                spItems.Children.Add(existingQuizItem);
            }

            var newQuizItem = new QuizItemControl(QuizTypes.CelebrityWords);
            newQuizItem.Add += QuizItem_Add;
            newQuizItem.Save += QuizItem_Save;
            newQuizItem.Delete += QuizItem_Delete;
            spItems.Children.Add(newQuizItem);
        }


        #region Event Handlers
        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.CelebrityWords);
        }

        private void QuizItem_Delete(string itemId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.CelebrityWodsQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            _contentStore.SelectedSegment?.CelebrityWodsQuiz.QuizItems.Remove(itemToRemove!);

            UpdateQuiz();
        }

        private void QuizItem_Save()
        {
            UpdateQuiz();
        }

        private void QuizItem_Add(QuizItem quizItem)
        {
            _contentStore.SelectedSegment?.CelebrityWodsQuiz.QuizItems.Add(quizItem);

            UpdateQuiz();
        }
        #endregion
    }
}
