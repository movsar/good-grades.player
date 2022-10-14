using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces {
    internal interface ICelebrityWords : IModelBase {
        // A set of materials, where each material has a personality image and words associated with it 
        Dictionary<string, KeyValuePair<byte[], string>> Data { get; set; }
    }
}