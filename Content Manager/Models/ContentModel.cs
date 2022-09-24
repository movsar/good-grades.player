using Data;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Models
{
    public class ContentModel
    {
        private readonly Storage _storage;
        public ContentModel(Storage storage)
        {
            _storage = storage;
            _storage.SegmentsRepository.ItemAdded += SegmentsRepository_ItemAdded;
            _storage.SegmentsRepository.ItemUpdated += SegmentsRepository_ItemUpdated;
        }

        private void SegmentsRepository_ItemUpdated(SegmentEntity dbEntity, IModelBase model)
        {
            dynamic segmentEntity = dbEntity;
            var segment = model as Segment;
            segment!.ReadingMaterials = _storage.SegmentsRepository.EntitiesToModels<ReadingMaterialEntity, ReadingMaterial>(segmentEntity.ReadingMaterials);
        }

        private void SegmentsRepository_ItemAdded(SegmentEntity dbEntity, IModelBase model)
        {

            // Set the Id for the inserted object
            model.Id = dbEntity.Id;
        }

        private IGeneralRepository SelectRepository<TModel>()
        {
            var t = typeof(TModel);
            switch (t)
            {
                case var _ when t.IsAssignableTo(typeof(ISegment)):
                case var _ when t.IsAssignableFrom(typeof(ISegment)):
                    return _storage.SegmentsRepository;

                default:
                    throw new Exception();
            }
        }
        public void DeleteItem<TModel>(IModelBase item) where TModel : IModelBase
        {
            SelectRepository<TModel>().Delete(item);
        }

        public void UpdateItem<TModel>(IModelBase item) where TModel : IModelBase
        {
            SelectRepository<TModel>().Update(item);
        }

        public void AddItem<TModel>(IModelBase item) where TModel : IModelBase
        {
            SelectRepository<TModel>().Add(item);
        }

        public IEnumerable<TModel> GetAll<TModel>() where TModel : IModelBase
        {
            return SelectRepository<TModel>().GetAll<TModel>();
        }
    }
}
