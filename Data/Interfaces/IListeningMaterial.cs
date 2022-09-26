using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IListeningMaterial : IModelBase
    {
        string Title { get; set; }
        string Content { get; set; }
        byte[] Audio { get; set; }
        byte[] Image { get; set; }
    }
}
