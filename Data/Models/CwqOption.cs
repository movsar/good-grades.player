using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models {
    internal class CwqOption {
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public byte[] Image { get; set; }
        public string WordsCollection { get; set; }
    }
}
