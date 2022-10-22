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
        public string Text { get; set; }

        public CwqOption(byte[] image, string wordsCollection) {
            Image = image;
            Text = wordsCollection;
        }

        public CwqOption() {
        }

        public CwqOption(CwqOptionEntity optionEntity) {
            Id = optionEntity.Id;
            CreatedAt = optionEntity.CreatedAt;
            ModifiedAt = optionEntity.ModifiedAt;

            Image = optionEntity.Image;
            Text = optionEntity.Text;
        }
    }
}
