using Content_Manager.Models;
using Data;
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

namespace Content_Manager.Stores {
    public class ContentStore {
        public List<ISegment> StoredSegments = new();

        private readonly ContentModel _contentModel;

        private Segment? _selectedSegment;
        public Segment? SelectedSegment {
            get {
                return _selectedSegment;
            }
            internal set {
                _selectedSegment = value;
                SelectedSegmentChanged?.Invoke(value);
            }
        }
        public event Action<Segment>? SelectedSegmentChanged;
        public ContentStore(ContentModel contentModel) {
            _contentModel = contentModel;
            LoadAllSegments();
        }

        public event Action<string, IModelBase>? ItemAdded;
        public event Action<string, IModelBase>? ItemUpdated;
        public event Action<string, IModelBase>? ItemDeleted;

        private void LoadAllSegments() {
            // Load from DB
            IEnumerable<ISegment> segmentsFromDb = _contentModel.GetAll<Segment>();

            // Refresh collection
            StoredSegments.Clear();
            foreach (ISegment segment in segmentsFromDb) {
                StoredSegments.Add(segment);
            }
        }
        internal TModel GetItemById<TModel>(string id) where TModel : IModelBase {
            // Add to DB
            return _contentModel.GetById<TModel>(id);
        }

        internal void UpdateCelebrityQuiz(ICelebrityWordsQuiz quiz) {
            // Save to DB
            _contentModel.UpdateItem<ICelebrityWordsQuiz>(quiz);
        }

        internal void UpdateSegment(ISegment segment) {
            // Update in runtime collection
            var index = StoredSegments.ToList().FindIndex(d => d.Id == segment.Id);
            StoredSegments[index] = segment;

            // Save to DB
            _contentModel.UpdateItem<ISegment>(segment);

            // Let everybody know
            ItemUpdated?.Invoke(nameof(ISegment), segment);
        }

        internal void AddSegment(ISegment item) {
            // Add to DB
            _contentModel.AddItem<ISegment>(item);

            // Add to collection
            StoredSegments.Add(item);

            // Let everybody know
            ItemAdded?.Invoke(nameof(ISegment), item);
        }

        public void DeleteSegment(ISegment item) {
            // Remove from DB
            _contentModel.DeleteItem<ISegment>(item);

            // Remove from collection
            var index = StoredSegments.ToList().FindIndex(d => d.Id == item.Id);
            StoredSegments.RemoveAt(index);

            // Let everybody know
            ItemDeleted?.Invoke(nameof(ISegment), item);
        }

        internal ISegment? FindSegmentByTitle(string segmentTitle) {
            return StoredSegments.Find(segment => segment.Title == segmentTitle);
        }

        public void DeleteSegments(IEnumerable<ISegment> itemsToDelete) {
            var immutableItems = itemsToDelete.ToList();
            foreach (var item in immutableItems) {
                DeleteSegment(item);
            }
        }

        internal ReadingMaterial GetReadingMaterialById(string id) {
            return SelectedSegment!.ReadingMaterials.Where(o => o.Id == id).First();
        }
        internal ListeningMaterial GetListeningMaterialById(string id) {
            return SelectedSegment!.ListeningMaterials.Where(o => o.Id == id).First();
        }

        internal CwqOption GetOptionById(string id) {
            return SelectedSegment!.CelebrityWodsQuiz.Options.Where(o => o.Id == id).First();
        }
    }
}
