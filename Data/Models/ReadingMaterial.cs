using Data.Interfaces;

namespace Data.Models
{
    public class ReadingMaterial : ModelBase, IReadingMaterial
    {
        private string _title;
        private string _content;

        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value, nameof(Title)); }
        }

		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value, nameof(Title)); }
		}

	}
}
