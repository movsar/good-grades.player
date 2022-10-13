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
                OnSegmentChange(value);
            }
        }
        public event Action<Segment>? SelectedSegmentChanged;
        public ContentStore(ContentModel contentModel) {
            _contentModel = contentModel;
            LoadAllSegments();
        }

        public event Action<IModelBase>? ItemAdded;
        public event Action<IModelBase>? ItemUpdated;
        public event Action<IModelBase>? ItemDeleted;

        private void OnSegmentChange(Segment segment) {
            SelectedSegmentChanged?.Invoke(segment);
        }
        private void OnItemAdded(IModelBase item) {
            ItemAdded?.Invoke(item);
        }
        private void OnItemUpdated(IModelBase item) {
            ItemUpdated?.Invoke(item);
        }
        private void OnItemDeleted(IModelBase item) {
            ItemDeleted?.Invoke(item);
        }

        private void LoadAllSegments() {
            // Load from DB
            IEnumerable<ISegment> segmentsFromDb = _contentModel.GetAll<Segment>();

            // Refresh collection
            StoredSegments.Clear();
            foreach (ISegment segment in segmentsFromDb) {
                StoredSegments.Add(segment);
            }
        }
        internal void UpdateItem<TModel>(IModelBase item) where TModel : IModelBase {
            // Update in runtime collection
            var index = StoredSegments.ToList().FindIndex(d => d.Id == item.Id);
            StoredSegments[index] = (ISegment)item;

            // Save to DB
            _contentModel.UpdateItem<TModel>(item);

            // Let everybody know
            OnItemUpdated(item);
        }
        internal void AddItem<TModel>(IModelBase item) where TModel : IModelBase {
            // Add to DB
            _contentModel.AddItem<TModel>(item);

            // Add to collection
            StoredSegments.Add((ISegment)item);

            // Let everybody know
            OnItemAdded(item);
        }

        public void DeleteItem<TModel>(TModel item) where TModel : IModelBase {
            // Remove from DB
            _contentModel.DeleteItem<TModel>(item);

            // Remove from collection
            var index = StoredSegments.ToList().FindIndex(d => d.Id == item.Id);
            StoredSegments.RemoveAt(index);

            // Let everybody know
            OnItemDeleted(item);
        }

        internal ISegment? FindSegmentByTitle(string segmentTitle) {
            return StoredSegments.Find(segment => segment.Title == segmentTitle);
        }

        public void DeleteItems<TModel>(IEnumerable<TModel> itemsToDelete) where TModel : IModelBase {
            var immutableItems = itemsToDelete.ToList();
            foreach (var item in immutableItems) {
                DeleteItem(item);
            }
        }

        internal ReadingMaterial GetReadingMaterialById(string id) {
            return SelectedSegment!.ReadingMaterials.Where(rm => rm.Id == id).First();
        }

        internal ListeningMaterial GetListeningMaterialById(string id) {
            return SelectedSegment!.ListeningMaterials.Where(rm => rm.Id == id).First();
        }
    }
}
