using Data.Entities.Materials;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class SegmentEntity : RealmObject, IEntityBase
    {
        [PrimaryKey] public string Id { get; } = ObjectId.GenerateNewId().ToString();
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        
        public IList<MatchingTaskMaterial> MatchingTasks { get; }
        public IList<SelectingTaskMaterial> SelectingTasks { get; }
        public IList<BuildingTaskMaterial> BuildingTasks { get; }
        public IList<FillingTaskMaterial> FillingTasks { get; }
        public IList<TestingTaskMaterial> TestingTasks { get; }

        public IList<ReadingMaterial> ReadingMaterials { get; }
        public IList<ListeningMaterial> ListeningMaterials { get; }

    }
}
