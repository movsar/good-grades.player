using Data.Interfaces;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class GeneralRepository<TEntity> : IGeneralRepository where TEntity : RealmObject, new()
    {
            private readonly Realm _realmInstance;

            internal GeneralRepository(Realm realm)
            {
                _realmInstance = realm;
            }

            #region Generic CRUD

            public TModel GetById<TModel>(string id)
            {
                var result = _realmInstance.Find<TEntity>(id);
                return EntitiesToModels<TEntity, TModel>(result);
            }

            public virtual void Add<TModel>(TModel model) where TModel : IModelBase
            {
                dynamic entity = new TEntity();
                entity.SetFromModel(model);

                _realmInstance.Write(() =>
                {
                    _realmInstance.Add(entity);
                });

                // Set the Id for the inserted object
                model.Id = entity.Id;
            }

            public virtual void Update<TModel>(TModel model) where TModel : IModelBase
            {
                dynamic entity = _realmInstance.Find<TEntity>(model.Id);
                _realmInstance.Write(() =>
                {
                    entity.SetFromModel(model);
                });
            }

            public virtual void Delete<TModel>(TModel model) where TModel : IModelBase
            {
                var entity = _realmInstance.Find<TEntity>(model.Id);
                _realmInstance.Write(() =>
                {
                    _realmInstance.Remove(entity);
                });
            }

            public virtual IEnumerable<TModel> GetAll<TModel>() where TModel : IModelBase
            {
                var entries = _realmInstance.All<TEntity>();
                return EntitiesToModels<TEntity, TModel>(entries);
            }

            #endregion

            #region EntitiesToModels

            // These method takes RealmObjects and turns them into plain model objects, works only for retrieval

            internal IEnumerable<TTarget> EntitiesToModels<TSource, TTarget>(IEnumerable<TSource> realmObjects)
            {
                string jsonString = JsonSerializer.Serialize(realmObjects);
                return JsonSerializer.Deserialize<IEnumerable<TTarget>>(jsonString);
            }
            public TTarget EntitiesToModels<TSource, TTarget>(TSource realmObject)
            {
                string jsonString = JsonSerializer.Serialize(realmObject);
                return JsonSerializer.Deserialize<TTarget>(jsonString);
            }

            #endregion

    }
}
