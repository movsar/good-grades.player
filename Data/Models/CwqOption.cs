using Data.Entities;
using Data.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models {
    public class CwqOption : ModelBase, ICwqOption {
        public byte[] Image { get; set; }
        public string WordsCollection { get; set; }

        public CwqOption(byte[] image, string wordsCollection) {
            Image = image;
            WordsCollection = wordsCollection;
        }

        public CwqOption() {
        }

        public CwqOption(CwqOptionEntity optionEntity) {
            Id = optionEntity.Id;
            CreatedAt = optionEntity.CreatedAt;
            ModifiedAt = optionEntity.ModifiedAt;

            Image = optionEntity.Image;
            WordsCollection = optionEntity.WordsCollection;
        }
    }
}
