using Data.Entities.Materials;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class Segment : RealmObject, IEntityBase
    {
        [PrimaryKey] public string Id { get; } = ObjectId.GenerateNewId().ToString();
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        
        public IList<MatchingTaskEntity> MatchingTasks { get; }
        public IList<SelectingTaskEntity> SelectingTasks { get; }
        public IList<BuildingTaskEntity> BuildingTasks { get; }
        public IList<FillingTaskEntity> FillingTasks { get; }
        public IList<TestingTaskEntity> TestingTasks { get; }

        public IList<ReadingAssignmentEntity> ReadingMaterials { get; }
        public IList<ListeningAssignmentEntity> ListeningMaterials { get; }

    }
}
