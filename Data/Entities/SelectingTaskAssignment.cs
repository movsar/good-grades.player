using Data.Entities.TaskItems;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("selecting_tasks")]
    public class SelectingTaskAssignment : IEntityBase, IAssignment
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        [Required] public string Title { get; set; }
        public Question Question { get; set; } = new Question();
        public bool IsContentSet => Question.Options.Count() > 0 && !string.IsNullOrEmpty(Question.CorrectOptionId);
    }
}
