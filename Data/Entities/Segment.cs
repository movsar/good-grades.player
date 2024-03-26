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

        public virtual IList<MatchingAssignment> MatchingTasks { get; set; } = new List<MatchingAssignment>();
        public virtual IList<SelectingAssignment> SelectingTasks { get; set; } = new List<SelectingAssignment>();
        public virtual IList<BuildingAssignment> BuildingTasks { get; set; } = new List<BuildingAssignment>();
        public virtual IList<FillingAssignment> FillingTasks { get; set; } = new List<FillingAssignment>();
        public virtual IList<TestingAssignment> TestingTasks { get; set; } = new List<TestingAssignment>();
        public virtual IList<Material> Materials { get; set; } = new List<Material>();
    }
}