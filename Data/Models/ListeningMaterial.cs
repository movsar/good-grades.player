using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ListeningMaterial : ModelBase, IListeningMaterial
    {
        private string _content;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value, nameof(Title)); }
        }

        private byte[] _audio;
        public byte[] Audio
        {
            get { return _audio; }
            set { SetProperty(ref _audio, value, nameof(Audio)); }
        }

        private byte[] _image;
        public byte[] Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value, nameof(Image)); }
        }
        
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value, nameof(Title)); }
        }
    }
}
