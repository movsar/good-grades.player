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

        public IList<MatchingTaskAssignment> MatchingTasks { get; set; } = new List<MatchingTaskAssignment>();
        public IList<SelectingTaskAssignment> SelectingTasks { get; set; } = new List<SelectingTaskAssignment>();
        public IList<BuildingTaskAssignment> BuildingTasks { get; set; } = new List<BuildingTaskAssignment>();
        public IList<FillingTaskAssignment> FillingTasks { get; set; } = new List<FillingTaskAssignment>();
        public IList<TestingTaskAssignment> TestingTasks { get; set; } = new List<TestingTaskAssignment>();

        public IList<ReadingMaterial> ReadingMaterials { get; set; } = new List<ReadingMaterial>();
        public IList<ListeningMaterial> ListeningMaterials { get; set; } = new List<ListeningMaterial>();
    }
}