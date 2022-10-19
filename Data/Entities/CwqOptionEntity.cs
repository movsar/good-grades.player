using Data.Interfaces;
using Data.Models;
using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities {
    public class CwqOptionEntity : RealmObject, ICwqOption, IEntityBase {
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public byte[] Image { get; set; }
        public string WordsCollection { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;

        public IModelBase AsModel() {
            return new CwqOption(this);
        }

        public void SetFromModel(IModelBase model) {
            var cwqOption = model as CwqOption;
            Image = cwqOption!.Image;
            WordsCollection = cwqOption!.WordsCollection;
        }
    }
}
