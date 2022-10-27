using Content_Manager.Models;
using Data;
using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Stores
{
    public class ContentStore
    {
        public event Action ContentStoreInitialized;

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
            _contentModel.ContentModelInitialized += ContentModelInitialized;
        }

        private void ContentModelInitialized()
        {
            LoadAllSegments();
            ContentStoreInitialized?.Invoke();
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
            // Remove from DB
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

        internal QuizItem GetQuizItem(string id)
        {
            var allQuizItems = SelectedSegment!.CelebrityWodsQuiz.QuizItems
                .Union(SelectedSegment!.ProverbSelectionQuiz.QuizItems)
                .Union(SelectedSegment!.ProverbBuilderQuiz.QuizItems)
                .Union(SelectedSegment!.GapFillerQuiz.QuizItems)
                .Union(SelectedSegment!.TestingQuiz.Questions.SelectMany(q => q.QuizItems));

            return allQuizItems.Where(o => o.Id == id).First();
        }


        internal void UpdateQuiz(QuizTypes quizType)
        {
            switch (quizType)
            {
                case QuizTypes.CelebrityWords:
                    _contentModel.UpdateItem<ICelebrityWordsQuiz>(SelectedSegment!.CelebrityWodsQuiz);
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

        internal void DeleteQuestionQuizItem(string quizItemId, TestingQuestion question)
        {
            // Deletes a quiz item associated with a testing question, checks if it was the correct one
            // and deals with that if so.

            var quizItemToRemove = question.QuizItems.Where(qi => qi.Id == quizItemId).First();
            question.QuizItems.Remove(quizItemToRemove);

            // If it's the only quiz item, make it correct by default 
            if (question.QuizItems.Count == 1 || (question.QuizItems.Count >= 1 && question.CorrectQuizId == quizItemId))
            {
                question.CorrectQuizId = question.QuizItems[0].Id;
            }
            else if (question.QuizItems.Count == 0)
            {
                question.CorrectQuizId = null;
            }

            _contentModel?.DeleteItem<QuizItem>(quizItemToRemove);
        }
    }
}
