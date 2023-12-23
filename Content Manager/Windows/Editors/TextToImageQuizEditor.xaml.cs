using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data.Entities;
using Data.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Content_Manager.Windows.Editors
{
    /// <summary>
    /// Interaction logic for TextToImageQuizEditor.xaml
    /// </summary>
    public partial class TextToImageQuizEditor : Window
    {

        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        private TextToImageQuizEntity _quiz;
        public TextToImageQuizEditor()
        {
            _quiz = new TextToImageQuizEntity();
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            //if (_contentStore?.SelectedSegment?.CelebrityWordsQuiz == null)
            //{
            //    return;
            //}
            //var previewWindow = new CelebrityQuizPresenter(_contentStore.SelectedSegment.CelebrityWordsQuiz);
            //previewWindow.ShowDialog();
        }


        #region Event Handlers
        private void UpdateQuiz()
        {
            //_contentStore.UpdateQuiz(QuizTypes.CelebrityWords);
            RedrawUi();
        }

        private void QuizItem_Delete(string itemId)
        {
            //var itemToRemove = _contentStore.SelectedSegment?.CelebrityWordsQuiz.QuizItems.Where(qi => qi.Id == itemId).First();
            //_contentStore.SelectedSegment?.CelebrityWordsQuiz.QuizItems.Remove(itemToRemove!);

            UpdateQuiz();
        }

        //private void QuizItem_Save(string? id, IModelBase model)
        //{
        //    UpdateQuiz();
        //}


        #endregion
        public void RedrawUi()
        {
            spItems.Children.Clear();
            //foreach (var quizItem in _contentStore.SelectedSegment!.CelebrityWordsQuiz.QuizItems)
            //{
            //    var existingQuizItemControl = new QuizItemControl(QuizTypes.CelebrityWords, quizItem);
            //    //existingQuizItemControl.Update += QuizItem_Save;
            //    //existingQuizItemControl.Delete += QuizItem_Delete;

            //    spItems.Children.Add(existingQuizItemControl);
            //}

            var newQuizItemControl = new QuizItemControl(QuizTypes.CelebrityWords);
            //newQuizItemControl.Create += QuizItem_Create;
            spItems.Children.Add(newQuizItemControl);
        }

        //private void QuizItem_Create(IModelBase model)
        //{
        //    _contentStore.SelectedSegment?.Cele.QuizItems.Add(model as QuizItem);
        //    UpdateQuiz();
        //}
    }
}
