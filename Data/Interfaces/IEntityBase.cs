using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IEntityBase : IRealmObject, IModelBase {
        public abstract void SetFromModel(IModelBase model);
    }
}
