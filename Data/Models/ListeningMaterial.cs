using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models {
    public class ListeningMaterial : ModelBase, IListeningMaterial {
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Audio { get; set; }
        public byte[] Image { get; set; }

        public ListeningMaterial() { }
        public ListeningMaterial(string title, string text, byte[] audio, byte[] image) {
            Title = title;
            Text = text;
            Audio = audio;
            Image = image;
        }

        public ListeningMaterial(ListeningMaterialEntity lmEntity) {
            Id = lmEntity.Id;
            CreatedAt = lmEntity.CreatedAt;
            ModifiedAt = lmEntity.ModifiedAt;

            Title = lmEntity.Title;
            Text = lmEntity.Text;
            Audio = lmEntity.Audio;
            Image = lmEntity.Image;
        }
    }
}
