using Data.Interfaces;
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ListeningMaterialEntity : IEntityBase, IListeningMaterial
    {
        public ListeningMaterialEntity() { }
        public ListeningMaterialEntity(ListeningMaterialEntity lm)
        {
            Id = lm.Id;
            Title = lm.Title;
            Text = lm.Text;
            Audio = lm.Audio;
            Image = lm.Image;
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Audio { get; set; }
        public byte[] Image { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;

        public IModelBase ToModel()
        {
            return new ListeningMaterial(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var o = model as IListeningMaterial;

            Title = o.Title;
            Text = o.Text;
            Image = o.Image;
            Audio = o.Audio;
            ModifiedAt = DateTime.Now;
        }
    }
}
