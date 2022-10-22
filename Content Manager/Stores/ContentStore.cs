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
            LoadAllSegments();
        }
        #endregion

        #region SegmentHandlers
        private void LoadAllSegments()
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

        internal QuizItem GetOptionById(string id)
        {
            return SelectedSegment!.CelebrityWodsQuiz.QuizItems.Where(o => o.Id == id).First();
        }

        internal void AddQuizItem(QuizTypes quizType, QuizItem newOption)
        {
            switch (quizType)
            {
                case QuizTypes.CelebrityWords:
                    SelectedSegment?.CelebrityWodsQuiz.QuizItems.Add(newOption);
                    _contentModel.UpdateItem<ICelebrityWordsQuiz>(SelectedSegment!.CelebrityWodsQuiz);
                    break;
                case QuizTypes.ProverbSelection:
                    SelectedSegment?.ProverbSelectionQuiz.QuizItems.Add(newOption);
                    _contentModel.UpdateItem<IProverbSelectionQuiz>(SelectedSegment!.ProverbSelectionQuiz);
                    break;
            }
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
            }
        }

        internal void DeleteQuizItem(QuizTypes quizType, string itemId)
        {
            var quizItem = GetOptionById(itemId);

            switch (quizType)
            {
                case QuizTypes.CelebrityWords:
                    SelectedSegment!.CelebrityWodsQuiz.QuizItems.Remove(quizItem);

                    _contentModel.UpdateItem<ICelebrityWordsQuiz>(SelectedSegment!.CelebrityWodsQuiz);

                    break;
                case QuizTypes.ProverbSelection:
                    SelectedSegment!.ProverbSelectionQuiz.QuizItems.Remove(quizItem);

                    _contentModel.UpdateItem<IProverbSelectionQuiz>(SelectedSegment!.ProverbSelectionQuiz);

                    break;
            }

        }
    }
}
