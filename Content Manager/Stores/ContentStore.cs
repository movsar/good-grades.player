using Content_Manager.Models;
using Data;
using Data.Entities;
using Data.Enums;
using Data.Interfaces;

using System;
using System.Collections.Generic;

using System.Linq;


namespace Content_Manager.Stores
{
    public class ContentStore
    {
        public event Action DatabaseInitialized;
        public event Action DatabaseUpdated;

        #region Properties
        public List<SegmentEntity> StoredSegments;
        private SegmentEntity? _selectedSegment;
        public SegmentEntity? SelectedSegment
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
        public event Action<SegmentEntity>? SelectedSegmentChanged;
        public event Action<string, IEntityBase>? ItemAdded;
        public event Action<string, IEntityBase>? ItemUpdated;
        public event Action<string, IEntityBase>? ItemDeleted;
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
            StoredSegments = _contentModel.Database.All<SegmentEntity>().ToList();
        }
        internal void UpdateSegment(SegmentEntity segment)
        {
            // Update in runtime collection
            var index = StoredSegments.ToList().FindIndex(d => d.Id == segment.Id);
            StoredSegments[index] = segment;

            // Save to DB
            _contentModel.UpdateItem<SegmentEntity>(segment);

            // Let everybody know
            ItemUpdated?.Invoke(nameof(SegmentEntity), segment);
        }
        internal void AddSegment(IModelBase item)
        {
            // Add to DB
            _contentModel.AddItem<SegmentEntity>(ref item);

            // Add to collection
            StoredSegments.Add(item as SegmentEntity);

            // Let everybody know
            ItemAdded?.Invoke(nameof(SegmentEntity), item);
        }
        public void DeleteSegment(SegmentEntity item)
        {
            StoredSegments.Remove(item);

            // Let everybody know
            ItemDeleted?.Invoke(nameof(SegmentEntity), item);
        }
        internal SegmentEntity? FindSegmentByTitle(string segmentTitle)
        {
            return StoredSegments.Find(segment => segment.Title == segmentTitle);
        }
        public void DeleteSegments(IEnumerable<SegmentEntity> itemsToDelete)
        {
            var immutableItems = itemsToDelete.ToList();
            foreach (var item in immutableItems)
            {
                DeleteSegment(item);
            }
        }
        #endregion

        internal ReadingMaterialEntity GetReadingMaterialById(string id)
        {
            return SelectedSegment!.ReadingMaterials.Where(o => o.Id == id).First();
        }

        internal ListeningMaterialEntity GetListeningMaterialById(string id)
        {
            return SelectedSegment!.ListeningMaterials.Where(o => o.Id == id).First();
        }

        internal TestingQuestionEntity? GetTestingQuestionByQuizItemId(string quizItemId)
        {
            //foreach (var question in SelectedSegment!.TestingQuizes.Question)
            //{
            //    if (question.QuizItems.Any(qi => qi.Id == quizItemId))
            //    {
            //        return question;
            //    }
            //}
            return null;
        }

        internal IEnumerable<QuizItemEntity> GetQuizItemsBySegment(SegmentEntity segment)
        {
            var allQuizItems = segment.CelebrityWordsQuiz.QuizItems
                .Union(segment.ProverbSelectionQuiz.QuizItems)
                .Union(segment.ProverbBuilderQuiz.QuizItems)
                .Union(segment.GapFillerQuiz.QuizItems)
                .Union(segment.TestingQuiz.Questions.SelectMany(q => q.QuizItems));

            return allQuizItems;
        }

        internal QuizItemEntity GetQuizItem(string id)
        {
            return GetQuizItemsBySegment(SelectedSegment!).Where(o => o.Id == id).First();
        }

        internal TestingQuestionEntity GetQuestionById(string id)
        {
            var question = SelectedSegment!.TestingQuizes!.Questions.Find(q => q.Id == id);
            return question!;
        }

        internal DbMetaEntity GetDbMeta()
        {
            return _contentModel.Database.All<DbMetaEntity>().First();
        }

        internal void SaveDbMeta(string title, string description)
        {
            var dbMeta = GetDbMeta();
            dbMeta.Title = title;
            dbMeta.Description = description;
            //_contentModel.UpdateItem<DbMeta>(dbMeta);

            // Let everybody know
            //ItemUpdated?.Invoke(nameof(IDbMeta), dbMeta);
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
            //var itemToRemove = SelectedSegment?.TestingQuiz.Questions.Where(qi => qi.Id == questionId).First();
            //SelectedSegment?.TestingQuiz.Questions.Remove(itemToRemove!);

            //_contentModel.DeleteItems<QuizItem>(itemToRemove.QuizItems);
            //_contentModel.DeleteItem<TestingQuestion>(itemToRemove!);
        }

        internal void DeleteSelectableQuizItem(QuizTypes quizType, string quizItemId, dynamic container)
        {
            // If remains only one item, mark it as a correct one
            // If the removed element was set as correct, set the first remaining as correct
            IEnumerable<QuizItemEntity> quizItems = container.QuizItems;

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

            //_contentModel?.DeleteItem<IQuizItem>(quizItemToRemove);
            //if (quizType == QuizTypes.Testing)
            //{
            //    _contentModel.UpdateItem<ITestingQuestion>(container);
            //}
            //if (quizType == QuizTypes.ProverbSelection)
            //{
            //    _contentModel.UpdateItem<IProverbSelectionQuiz>(container);
            //}
        }

        internal void DeleteListeningMaterial(string id)
        {
            var listeningMaterial = GetListeningMaterialById(id);
            SelectedSegment!.ListeningMaterials.Remove(listeningMaterial);
            //_contentModel!.DeleteItem<ListeningMaterialEntity>(listeningMaterial);
            SaveCurrentSegment();
        }

        internal void DeleteReadingMaterial(string id)
        {
            var readingMaterial = GetReadingMaterialById(id);
            SelectedSegment!.ReadingMaterials.Remove(readingMaterial);
            //_contentModel!.DeleteItem<ReadingMaterialEntity>(readingMaterial);
            SaveCurrentSegment();
        }

        internal void CreateSelectableQuizItem(QuizTypes quizType, QuizItemEntity? quizItem, dynamic container)
        {
            container.QuizItems.Add(quizItem);
            //UpdateQuiz(quizType);

            // If it's the only quiz item, make it correct by default 
            if (container.QuizItems.Count == 1)
            {
                container.CorrectQuizId = quizItem.Id;
            }
            //UpdateQuiz(quizType);
        }

        internal void ImportDatabase(string filePath)
        {
            _contentModel.ImportDatabase(filePath);
        }
    }
}
