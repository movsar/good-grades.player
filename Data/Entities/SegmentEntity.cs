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
    public class SegmentEntity : RealmObject, ISegment, IEntityBase
    {
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<ReadingMaterialEntity> ReadingMaterials { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public void SetFromModel(IModelBase model)
        {
            var segment = model as Segment;
            Title = segment.Title;
            Description = segment.Description;
            ModifiedAt = DateTime.Now;

            if (segment.ReadingMaterials == null)
            {
                return;
            }

            foreach (var material in segment.ReadingMaterials)
            {
                var readingMaterialEntity = new ReadingMaterialEntity();
                readingMaterialEntity.SetFromModel(material);
                ReadingMaterials.Add(readingMaterialEntity);
            }
        }
    }
}
