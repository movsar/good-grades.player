using Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Segment : IEntityBase
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        
        public IList<MatchingTaskAssignment> MatchingTasks { get; }
        public IList<SelectingTaskAssignment> SelectingTasks { get; }
        public IList<BuildingTaskAssignment> BuildingTasks { get; }
        public IList<FillingTaskAssignment> FillingTasks { get; }
        public IList<TestingTaskAssignment> TestingTasks { get; }

        public IList<ReadingMaterial> ReadingMaterials { get; }
        public IList<ListeningMaterial> ListeningMaterials { get; }

    }
}
