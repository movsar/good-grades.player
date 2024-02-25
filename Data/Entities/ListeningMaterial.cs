using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("listening_tasks")]
    public class ListeningMaterial : IEntityBase, IMaterial
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Audio { get; set; }
        public byte[] Image { get; set; }
    }
}
