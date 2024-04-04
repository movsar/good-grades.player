using Castle.Core.Internal;
using Data.Entities.TaskItems;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("selecting_assignments")]
    public class SelectingAssignment : IEntityBase, IAssignment
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        [Required] public string Title { get; set; }
        public virtual Question Question { get; set; } = new Question();
        public bool IsContentSet => Question.Options.Count() > 1 && Question.Options.Any(o => o.IsChecked) == true;
    }
}