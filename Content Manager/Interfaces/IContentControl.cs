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
        public event Action<string?, IModelBase> Save;
        public event Action<string> Delete;
    }
}
