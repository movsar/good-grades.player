using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models {
    public class ListeningMaterial : ModelBase, IListeningMaterial {
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Audio { get; set; }
        public byte[] Image { get; set; }

        public ListeningMaterial() { }
        public ListeningMaterial(string title, string content, byte[] audio, byte[] image) {
            Title = title;
            Content = content;
            Audio = audio;
            Image = image;
        }
    }
}
