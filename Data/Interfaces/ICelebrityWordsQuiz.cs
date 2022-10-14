using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces {
    public interface ICelebrityWordsQuiz : IModelBase {
        string SegmentId { get; set; }
        
        // A set of materials, where each material has a personality image and words associated with it 
        Dictionary<string, KeyValuePair<byte[], string>> Data { get; set; }
    }
}