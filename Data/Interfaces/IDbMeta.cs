using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IDbMeta : IModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
