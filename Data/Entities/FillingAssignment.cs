using Data.Entities.TaskItems;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("filling_assignments")]
    public class FillingAssignment : IEntityBase, IAssignment
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Title { get; set; }
        public virtual IList<AssignmentItem> Items { get; } = new List<AssignmentItem>();
        public bool IsContentSet => Items.Count() > 0;
    }
}
