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
        public IList<ReadingMaterialEntity> ReadingMaterials { get; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public void SetFromModel(IModelBase model)
        {
            // Set segment info
            var segment = model as Segment;
            Title = segment.Title;
            Description = segment.Description;
            ModifiedAt = DateTime.Now;

            if (segment.ReadingMaterials == null)
            {
                return;
            }

            // Commit removed reading materials
            var currentReadingMaterialsIds = segment.ReadingMaterials.Select(x => x.Id).ToList();
            var readingMaterialsToRemove = ReadingMaterials.Where(rm => !currentReadingMaterialsIds.Contains(rm.Id));
            foreach (var readingMaterial in readingMaterialsToRemove)
            {
                ReadingMaterials.Remove(readingMaterial);
            }

            // Add or update reading materials
            foreach (var material in segment.ReadingMaterials)
            {
                var existingReadingMaterial = ReadingMaterials?.FirstOrDefault((rm => rm.Id == material.Id));
                if (existingReadingMaterial != null)
                {
                    existingReadingMaterial.SetFromModel(material);
                }
                else
                {
                    var newReadingMaterial = new ReadingMaterialEntity();
                    newReadingMaterial.SetFromModel(material);
                    ReadingMaterials?.Add(newReadingMaterial);
                }
            }
        }
    }
}
