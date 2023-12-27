using Data.Entities.Materials;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class SegmentEntity : RealmObject, IEntityBase
    {
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        public IList<MatchingTaskMaterial> TextToImageQuizes { get; }
        public IList<SelectingTaskMaterial> ProverbSelectionQuizes { get; }
        public IList<BuildingTaskMaterial> ProverbBuilderQuizes { get; }
        public IList<FillingTaskMaterial> GapFillerQuizes { get; }
        public IList<TestingTaskMaterial> TestingQuizes { get; }
        public IList<ReadingMaterial> ReadingMaterials { get; }
        public IList<ListeningMaterial> ListeningMaterials { get; }

    }
}
