using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public abstract class GeneralRepository<TEntity> : IGeneralRepository where TEntity : IEntityBase, new()
    {
        private readonly Realm _realmInstance;

        internal GeneralRepository(Realm realm)
        {
            _realmInstance = realm;
        }

        protected IEnumerable<TEntity> GetEntitiesByIds(string[] ids)
        {
            foreach (var entity in _realmInstance.All<TEntity>())
            {
                if (ids.Any(id => id == entity.Id))
                {
                    yield return entity;
                }
            }
        }

        #region Generic CRUD

        public virtual void Add<TModel>(ref TModel model) where TModel : IModelBase
        {
            dynamic entity = new TEntity();
            entity.SetFromModel(model);

            _realmInstance.Write(() =>
            {
                _realmInstance.Add(entity);
            });

            model = entity.ToModel();
        }

        public virtual void Update<TModel>(TModel model) where TModel : IModelBase
        {
            dynamic entity = _realmInstance.Find<TEntity>(model.Id);
            _realmInstance.Write(() =>
            {
                entity.SetFromModel(model);
            });

            model = entity.ToModel();
        }

        public virtual void Delete<TModel>(TModel model) where TModel : IModelBase
        {
            var entity = _realmInstance.Find<TEntity>(model.Id);
            _realmInstance.Write(() =>
            {
                _realmInstance.Remove(entity);
            });
        }

        public TModel GetById<TModel>(string id)
        {
            var result = _realmInstance.Find<TEntity>(id);
            return (TModel)result.ToModel();
        }

        public virtual IEnumerable<TModel> GetByIds<TModel>(string[] ids) where TModel : IModelBase
        {
            var entries = GetEntitiesByIds(ids);
            return EntitiesToModels<TEntity, TModel>(entries);
        }

        public virtual IEnumerable<TModel> GetAll<TModel>() where TModel : IModelBase
        {
            var entries = _realmInstance.All<TEntity>();
            return EntitiesToModels<TEntity, TModel>(entries);
        }
        #endregion

        public IEnumerable<TTarget> EntitiesToModels<TSource, TTarget>(IEnumerable<TSource> realmObjects)
        {
            var collection = (IEnumerable<IEntityBase>)realmObjects;

            foreach (var item in collection)
            {
                yield return (TTarget)item.ToModel();
            }
        }
    }
}
