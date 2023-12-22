using Data.Interfaces;
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ReadingMaterialEntity : IEntityBase, IReadingMaterial
    {
        public ReadingMaterialEntity() { }
        public ReadingMaterialEntity(ReadingMaterialEntity rm)
        {
            Id = rm.Id;
            Title = rm.Title;
            Text = rm.Text;
            Image = rm.Image;
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;

        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }

        public IModelBase ToModel()
        {
            return new ReadingMaterial(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var o = model as IReadingMaterial;

            Title = o.Title;
            Text = o.Text;
            Image = o.Image;
            ModifiedAt = DateTime.Now;
        }
    }
}
