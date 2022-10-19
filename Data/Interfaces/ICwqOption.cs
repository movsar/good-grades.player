using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces {
    public interface ICwqOption : IModelBase {
        public byte[] Image { get; set; }
        public string WordsCollection { get; set; }
    }
}
