using Content_Manager.Stores;
using Content_Manager.UserControls;
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
using System.Windows.Shapes;

namespace Content_Manager.Windows.Editors
{
    /// <summary>
    /// Interaction logic for TextToImageQuizEditor.xaml
    /// </summary>
    public partial class TextToImageQuizEditor : Window
    {

        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        private TextToImageQuiz _quiz;
        public TextToImageQuizEditor()
        {
            _quiz = new TextToImageQuiz();
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (_contentStore?.SelectedSegment?.CelebrityWordsQuiz == null)
            {
                return;
            }
            var previewWindow = new CelebrityQuizPresenter(_contentStore.SelectedSegment.CelebrityWordsQuiz);
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
            var itemToRemove = _contentStore.SelectedSegment?.CelebrityWordsQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            _contentStore.SelectedSegment?.CelebrityWordsQuiz.QuizItems.Remove(itemToRemove!);

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
            foreach (var quizItem in _contentStore.SelectedSegment!.CelebrityWordsQuiz.QuizItems)
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
            _contentStore.SelectedSegment?.Cele.QuizItems.Add(model as QuizItem);
            UpdateQuiz();
        }
    }
}
