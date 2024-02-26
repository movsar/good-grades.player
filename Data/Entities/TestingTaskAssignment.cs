using Data.Entities.TaskItems;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("testing_tasks")]
    public class TestingTaskAssignment : IEntityBase, IAssignment
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        [Required] public string Title { get; set; }
        public virtual IList<Question> Questions { get; } = new List<Question>();
        public bool IsContentSet => Questions.Count() > 0;
    }
}
