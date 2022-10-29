using Data.Interfaces;
using Data.Models;
using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ListeningMaterialEntity : RealmObject, IEntityBase, IListeningMaterial
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

        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

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
