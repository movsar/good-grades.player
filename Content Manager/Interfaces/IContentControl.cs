using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content_Manager.Interfaces
{
    internal interface IMaterialControl
    {
        public event Action<IModelBase> Create;
        public event Action<string?, IModelBase> Update;
        public event Action<string> Delete;
    }
}
