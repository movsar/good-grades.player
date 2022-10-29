using Content_Manager.Models;
using Data;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Stores
{
    public class ContentStore
    {
        public event Action DatabaseInitialized;
        public event Action DatabaseUpdated;

        #region Properties
        public List<ISegment> StoredSegments = new();
        private Segment? _selectedSegment;
        public Segment? SelectedSegment
        {
            get
            {
                return _selectedSegment;
            }
            internal set
            {
                _selectedSegment = value;
                SelectedSegmentChanged?.Invoke(value);
            }
        }
        #endregion

        #region Fields
        private readonly ContentModel _contentModel;
        #endregion

        #region Events
        public event Action<Segment>? SelectedSegmentChanged;
        public event Action<string, IModelBase>? ItemAdded;
        public event Action<string, IModelBase>? ItemUpdated;
        public event Action<string, IModelBase>? ItemDeleted;
        #endregion

        #region Initialization
        public ContentStore(ContentModel contentModel)
        {
            _contentModel = contentModel;
            _contentModel.DatabaseInitialized += ContentModelInitialized;
            _contentModel.DatabaseUpdated += OnDatabaseUpdated;
        }

        private void OnDatabaseUpdated()
        {
            LoadAllSegments();
            DatabaseUpdated?.Invoke();
        }

        private void ContentModelInitialized()
        {
            LoadAllSegments();
            DatabaseInitialized?.Invoke();
        }
        #endregion

        #region SegmentHandlers
        public void LoadAllSegments()
        {
            // Load from DB
            IEnumerable<ISegment> segmentsFromDb = _contentModel.GetAll<Segment>();

            // Refresh collection
            StoredSegments.Clear();
            foreach (ISegment segment in segmentsFromDb)
            {
                StoredSegments.Add(segment);
            }
        }
        internal void UpdateSegment(ISegment segment)
        {
            // Update in runtime collection
            var index = StoredSegments.ToList().FindIndex(d => d.Id == segment.Id);
            StoredSegments[index] = segment;

            // Save to DB
            _contentModel.UpdateItem<ISegment>(segment);

            // Let everybody know
            ItemUpdated?.Invoke(nameof(ISegment), segment);
        }
        internal void AddSegment(IModelBase item)
        {
            // Add to DB
            _contentModel.AddItem<ISegment>(ref item);

            // Add to collection
            StoredSegments.Add(item as ISegment);

            // Let everybody know
            ItemAdded?.Invoke(nameof(ISegment), item);
        }
        public void DeleteSegment(ISegment item)
        {
            var segment = item as Segment;

            // Delete related quiz items
            var quizItems = GetQuizItemsBySegment(segment);
            _contentModel.DeleteItems<IQuizItem>(quizItems);

            // Delete related test questions
            _contentModel.DeleteItems<ITestingQuestion>(segment.TestingQuiz.Questions);

            // Delete related quizes
            _contentModel.DeleteItem<ICelebrityWordsQuiz>(segment!.CelebrityWordsQuiz);
            _contentModel.DeleteItem<IProverbSelectionQuiz>(segment!.ProverbSelectionQuiz);
            _contentModel.DeleteItem<IProverbBuilderQuiz>(segment!.ProverbBuilderQuiz);
            _contentModel.DeleteItem<IGapFillerQuiz>(segment!.GapFillerQuiz);
            _contentModel.DeleteItem<ITestingQuiz>(segment!.TestingQuiz);

            // Delete related reading and listening materials
            _contentModel.DeleteItems<IReadingMaterial>(segment.ReadingMaterials);
            _contentModel.DeleteItems<IListeningMaterial>(segment.ListeningMaterials);

            // Delete the segment
            _contentModel.DeleteItem<ISegment>(item);

            // Remove from collection
            var index = StoredSegments.ToList().FindIndex(d => d.Id == item.Id);
            StoredSegments.RemoveAt(index);

            // Let everybody know
            ItemDeleted?.Invoke(nameof(ISegment), item);
        }
        internal ISegment? FindSegmentByTitle(string segmentTitle)
        {
            return StoredSegments.Find(segment => segment.Title == segmentTitle);
        }
        public void DeleteSegments(IEnumerable<ISegment> itemsToDelete)
        {
            var immutableItems = itemsToDelete.ToList();
            foreach (var item in immutableItems)
            {
                DeleteSegment(item);
            }
        }
        #endregion

        internal ReadingMaterial GetReadingMaterialById(string id)
        {
            return SelectedSegment!.ReadingMaterials.Where(o => o.Id == id).First();
        }

        internal ListeningMaterial GetListeningMaterialById(string id)
        {
            return SelectedSegment!.ListeningMaterials.Where(o => o.Id == id).First();
        }

        internal TestingQuestion? GetTestingQuestionByQuizItemId(string quizItemId)
        {
            foreach (var question in SelectedSegment!.TestingQuiz.Questions)
            {
                if (question.QuizItems.Any(qi => qi.Id == quizItemId))
                {
                    return question;
                }
            }
            return null;
        }

        internal IEnumerable<QuizItem> GetQuizItemsBySegment(Segment segment)
        {
            var allQuizItems = segment.CelebrityWordsQuiz.QuizItems
                .Union(segment.ProverbSelectionQuiz.QuizItems)
                .Union(segment.ProverbBuilderQuiz.QuizItems)
                .Union(segment.GapFillerQuiz.QuizItems)
                .Union(segment.TestingQuiz.Questions.SelectMany(q => q.QuizItems));

            return allQuizItems;
        }

        internal QuizItem GetQuizItem(string id)
        {
            return GetQuizItemsBySegment(SelectedSegment!).Where(o => o.Id == id).First();
        }

        internal void UpdateQuiz(QuizTypes quizType)
        {
            switch (quizType)
            {
                case QuizTypes.CelebrityWords:
                    _contentModel.UpdateItem<ICelebrityWordsQuiz>(SelectedSegment!.CelebrityWordsQuiz);
                    break;

                case QuizTypes.ProverbSelection:
                    _contentModel.UpdateItem<IProverbSelectionQuiz>(SelectedSegment!.ProverbSelectionQuiz);
                    break;

                case QuizTypes.ProverbBuilder:
                    _contentModel.UpdateItem<IProverbBuilderQuiz>(SelectedSegment!.ProverbBuilderQuiz);
                    break;

                case QuizTypes.GapFiller:
                    _contentModel.UpdateItem<IGapFillerQuiz>(SelectedSegment!.GapFillerQuiz);
                    break;

                case QuizTypes.Testing:
                    _contentModel.UpdateItem<ITestingQuiz>(SelectedSegment!.TestingQuiz);
                    break;
            }

            //SelectedSegment = SelectedSegment;
        }

        internal TestingQuestion GetQuestionById(string id)
        {
            var question = SelectedSegment!.TestingQuiz!.Questions.Find(q => q.Id == id);
            return question!;
        }

        internal DbMeta GetDbMeta()
        {
            return _contentModel.GetAll<DbMeta>().First();
        }

        internal void SaveDbMeta(string title, string description)
        {
            var dbMeta = GetDbMeta();
            dbMeta.Title = title;
            dbMeta.Description = description;
            _contentModel.UpdateItem<DbMeta>(dbMeta);

            // Let everybody know
            ItemUpdated?.Invoke(nameof(IDbMeta), dbMeta);
        }

        internal void OpenDatabase(string filePath)
        {
            _contentModel.OpenDatabase(filePath);
        }
        internal void CreateDatabase(string filePath)
        {
            _contentModel.CreateDatabase(filePath);
        }
        internal void SaveCurrentSegment()
        {
            UpdateSegment(SelectedSegment!);
        }

        internal void DeleteQuestion(string questionId)
        {
            var itemToRemove = SelectedSegment?.TestingQuiz.Questions.Where(qi => qi.Id == questionId).First();
            SelectedSegment?.TestingQuiz.Questions.Remove(itemToRemove!);

            _contentModel.DeleteItems<QuizItem>(itemToRemove.QuizItems);
            _contentModel.DeleteItem<TestingQuestion>(itemToRemove!);
        }

        internal void DeleteSelectableQuizItem(QuizTypes quizType, string quizItemId, dynamic container)
        {
            // If remains only one item, mark it as a correct one
            // If the removed element was set as correct, set the first remaining as correct
            IEnumerable<QuizItem> quizItems = container.QuizItems;

            var quizItemToRemove = quizItems.Where(qi => qi.Id == quizItemId).First();
            container.QuizItems.Remove(quizItemToRemove);

            if (container.QuizItems.Count == 1 || (container.QuizItems.Count >= 1 && container.CorrectQuizId == quizItemId))
            {
                container.CorrectQuizId = container.QuizItems[0].Id;
            }
            else if (container.QuizItems.Count == 0)
            {
                container.CorrectQuizId = null;
            }

            _contentModel?.DeleteItem<IQuizItem>(quizItemToRemove);
            if (quizType == QuizTypes.Testing)
            {
                _contentModel.UpdateItem<ITestingQuestion>(container);
            }
            if (quizType == QuizTypes.ProverbSelection)
            {
                _contentModel.UpdateItem<IProverbSelectionQuiz>(container);
            }
        }

        internal void DeleteListeningMaterial(string id)
        {
            var listeningMaterial = GetListeningMaterialById(id);
            SelectedSegment!.ListeningMaterials.Remove(listeningMaterial);
            _contentModel!.DeleteItem<ListeningMaterial>(listeningMaterial);
            SaveCurrentSegment();
        }

        internal void DeleteReadingMaterial(string id)
        {
            var readingMaterial = GetReadingMaterialById(id);
            SelectedSegment!.ReadingMaterials.Remove(readingMaterial);
            _contentModel!.DeleteItem<ReadingMaterial>(readingMaterial);
            SaveCurrentSegment();
        }

        internal void CreateSelectableQuizItem(QuizTypes quizType, QuizItem? quizItem, dynamic container)
        {
            container.QuizItems.Add(quizItem);
            UpdateQuiz(quizType);

            // If it's the only quiz item, make it correct by default 
            if (container.QuizItems.Count == 1)
            {
                container.CorrectQuizId = quizItem.Id;
            }
            UpdateQuiz(quizType);
        }

        internal void ImportDatabase(string filePath)
        {
            _contentModel.ImportDatabase(filePath);
        }
    }
}
