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

namespace Content_Manager.Stores
{
    public class ContentStore
    {
        public List<ISegment> StoredSegments = new();

        private readonly ContentModel _contentModel;
        public ContentStore(ContentModel contentModel)
        {
            _contentModel = contentModel;
            LoadAllSegments();
        }

        public event Action<IModelBase>? ItemAdded;
        public event Action<IModelBase>? ItemUpdated;
        public event Action<IModelBase>? ItemDeleted;

        private void OnItemAdded(IModelBase item, string collectionName)
        {
            ItemAdded?.Invoke(item);
        }
        private void OnItemUpdated(IModelBase item, string collectionName)
        {
            ItemUpdated?.Invoke(item);
        }
        private void OnItemDeleted(IModelBase item, string collectionName)
        {
            ItemDeleted?.Invoke(item);
        }

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
        private (string, IList<TModel>) SelectCollection<TModel>() where TModel : IModelBase
        {
            var t = typeof(TModel);
            switch (t)
            {
                case var _ when t.IsAssignableTo(typeof(ISegment)):
                case var _ when t.IsAssignableFrom(typeof(ISegment)):
                    return new(nameof(StoredSegments), (IList<TModel>)StoredSegments);

                default:
                    throw new Exception();
            }
        }
        internal void UpdateItem<TModel>(IModelBase item) where TModel : IModelBase
        {
            // Update in runtime collection
            var (collectionName, items) = SelectCollection<TModel>();
            var index = items.ToList().FindIndex(d => d.Id == item.Id);
            items[index] = (TModel)item;

            // Save to DB
            _contentModel.UpdateItem<TModel>(item);

            // Let everybody know
            OnItemUpdated(item, collectionName);
        }
        internal void AddItem<TModel>(IModelBase item) where TModel : IModelBase
        {
            // Add to DB
            _contentModel.AddItem<TModel>(item);

            // Add to collection
            var (collectionName, items) = SelectCollection<TModel>();
            items.Add((TModel)item);

            // Let everybody know
            OnItemAdded(item, collectionName);
        }

        public void DeleteItem<TModel>(TModel item) where TModel : IModelBase
        {
            // Remove from DB
            _contentModel.DeleteItem<TModel>(item);

            // Remove from collection
            var (collectionName, items) = SelectCollection<TModel>();
            var index = items.ToList().FindIndex(d => d.Id == item.Id);
            items.RemoveAt(index);

            // Let everybody know
            OnItemDeleted(item, collectionName);
        }

        internal ISegment? FindSegmentByTitle(string segmentTitle)
        {
            return StoredSegments.Find(segment => segment.Title == segmentTitle);
        }

        public void DeleteItems<TModel>(IEnumerable<TModel> itemsToDelete) where TModel : IModelBase
        {
            var immutableItems = itemsToDelete.ToList();
            foreach (var item in immutableItems)
            {
                DeleteItem(item);
            }
        }
    }
}
