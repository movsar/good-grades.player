using Data.Interfaces;
using System;

namespace Content_Manager.Interfaces
{
    internal interface IMaterialControl
    {
        public event Action<IEntityBase> Create;
        public event Action<string?, IEntityBase> Update;
        public event Action<string> Delete;
    }
}
