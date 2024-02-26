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
        public string? Description { get; set; }

        public virtual IList<MatchingTaskAssignment> MatchingTasks { get; set; } = new List<MatchingTaskAssignment>();
        public virtual IList<SelectingTaskAssignment> SelectingTasks { get; set; } = new List<SelectingTaskAssignment>();
        public virtual IList<BuildingTaskAssignment> BuildingTasks { get; set; } = new List<BuildingTaskAssignment>();
        public virtual IList<FillingTaskAssignment> FillingTasks { get; set; } = new List<FillingTaskAssignment>();
        public virtual IList<TestingTaskAssignment> TestingTasks { get; set; } = new List<TestingTaskAssignment>();
        public virtual IList<ReadingMaterial> ReadingMaterials { get; set; } = new List<ReadingMaterial>();
        public virtual IList<ListeningMaterial> ListeningMaterials { get; set; } = new List<ListeningMaterial>();
    }
}