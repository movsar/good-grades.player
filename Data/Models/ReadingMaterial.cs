using Data.Interfaces;

namespace Data.Models {
    public class ReadingMaterial : ModelBase, IReadingMaterial {
        public string Content { get; set; }
        public string Title { get; set; }

        public ReadingMaterial(string title, string content) {
            Title = title;
            Content = content;
        }
    }
}
