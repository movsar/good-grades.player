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

namespace Content_Manager.UserControls.Tabs
{
    public partial class ProverbSelectionQuizTab : UserControl
    {
        private readonly ContentStore _contentStore;
        public ProverbSelectionQuizTab()
        {
            InitializeComponent();
            DataContext = this;

            _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
            _contentStore.SelectedSegmentChanged += _contentStore_SegmentChanged;
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

        private void _contentStore_SegmentChanged(Segment selectedSegment)
        {
            if (selectedSegment == null) return;

            spItems.Children.Clear();

            foreach (var quizItem in selectedSegment.ProverbSelectionQuiz.QuizItems)
            {
                var existingQuizItem = new QuizItemControl(QuizTypes.ProverbSelection, quizItem);
                existingQuizItem.Add += QuizItem_Add;
                existingQuizItem.Save += QuizItem_Save;
                existingQuizItem.Delete += QuizItem_Delete;

                spItems.Children.Add(existingQuizItem);
            }

            var newQuizItem = new QuizItemControl(QuizTypes.ProverbSelection);
            newQuizItem.Add += QuizItem_Add;
            newQuizItem.Save += QuizItem_Save;
            newQuizItem.Delete += QuizItem_Delete;

            spItems.Children.Add(newQuizItem);
        }

        #region Event Handlers
        private void UpdateQuiz()
        {
            _contentStore.UpdateQuiz(QuizTypes.ProverbSelection);
        }

        private void QuizItem_Delete(string itemId)
        {
            var itemToRemove = _contentStore.SelectedSegment?.ProverbSelectionQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            _contentStore.SelectedSegment?.ProverbSelectionQuiz.QuizItems.Remove(itemToRemove!);

            UpdateQuiz();
        }

        private void QuizItem_Save()
        {
            UpdateQuiz();
        }

        private void QuizItem_Add(QuizItem quizItem)
        {
            _contentStore.SelectedSegment?.ProverbSelectionQuiz.QuizItems.Add(quizItem);

            UpdateQuiz();
        }
        #endregion

    }
}
