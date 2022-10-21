using Data.Entities;
using Data.Interfaces;

namespace Data.Models {
    public class ReadingMaterial : ModelBase, IReadingMaterial {
        public string Text { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public ReadingMaterial(string title, string text, byte[] image)
        {
            Title = title;
            Text = text;
            Image = image;
        }
        public ReadingMaterial() { }

        public ReadingMaterial(ReadingMaterialEntity rmEntity) {
            Id = rmEntity.Id;
            CreatedAt = rmEntity.CreatedAt;
            ModifiedAt = rmEntity.ModifiedAt;

            Text = rmEntity.Text;
            Title = rmEntity.Title;
            Image = rmEntity.Image;
        }
    }
}
