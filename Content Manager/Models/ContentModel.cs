using Data;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Models {
    public class ContentModel {
        private readonly Storage _storage;
        public ContentModel(Storage storage) {
            _storage = storage;
        }

        private IGeneralRepository SelectRepository<TModel>() {
            var t = typeof(TModel);
            switch (t) {
                case var _ when t.IsAssignableTo(typeof(ISegment)):
                case var _ when t.IsAssignableFrom(typeof(ISegment)):
                    return _storage.SegmentsRepository;
                case var _ when t.IsAssignableTo(typeof(ICelebrityWordsQuiz)):
                case var _ when t.IsAssignableFrom(typeof(ICelebrityWordsQuiz)):
                    return _storage.CwqRepository;

                default:
                    throw new Exception();
            }
        }
        public void DeleteItem<TModel>(IModelBase item) where TModel : IModelBase {
            SelectRepository<TModel>().Delete(item);
        }

        public void UpdateItem<TModel>(IModelBase item) where TModel : IModelBase {
            SelectRepository<TModel>().Update(item);
        }

        public void AddItem<TModel>(IModelBase item) where TModel : IModelBase {
            SelectRepository<TModel>().Add(item);
        }

        public TModel GetById<TModel>(string id) where TModel : IModelBase {
            return SelectRepository<TModel>().GetById<TModel>(id);
        }

        public IEnumerable<TModel> GetAll<TModel>() where TModel : IModelBase {
            return SelectRepository<TModel>().GetAll<TModel>();
        }
    }
}
