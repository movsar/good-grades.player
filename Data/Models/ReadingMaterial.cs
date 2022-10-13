using Data.Interfaces;

namespace Data.Models {
    public class ReadingMaterial : ModelBase, IReadingMaterial {
        public string Text { get; set; }
        public string Title { get; set; }

        public ReadingMaterial(string title, string text) {
            Title = title;
            Text = text;
        }
    }
}
