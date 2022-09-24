using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IGeneralRepository
    {
        void Add<TModel>(TModel model) where TModel : IModelBase;
        void Delete<TModel>(TModel model) where TModel : IModelBase;
        IEnumerable<TModel> GetAll<TModel>() where TModel : IModelBase;
        TModel GetById<TModel>(string id);
        void Update<TModel>(TModel model) where TModel : IModelBase;
    }
}
