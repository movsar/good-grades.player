using Data.Interfaces;

using Realms;

namespace Data.Entities
{
    public class Segment : RealmObject, IEntityBase
    {
        [PrimaryKey] public string Id { get; } = Guid.NewGuid().ToString();
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        
        public IList<MatchingTaskAssignment> MatchingTasks { get; }
        public IList<SelectingTaskAssignment> SelectingTasks { get; }
        public IList<BuildingTaskAssignment> BuildingTasks { get; }
        public IList<FillingTaskAssignment> FillingTasks { get; }
        public IList<TestingTaskAssignment> TestingTasks { get; }

        public IList<ReadingMaterial> ReadingMaterials { get; }
        public IList<ListeningMaterial> ListeningMaterials { get; }

    }
}
