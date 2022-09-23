using Data.Interfaces;
using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ReadingMaterialEntity : RealmObject, IEntityBase, IReadingMaterial
    {
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public void SetFromModel(IModelBase model)
        {
            var o = model as IReadingMaterial;
            Content = o.Content;
            ModifiedAt = DateTime.Now;
        }
    }
}
