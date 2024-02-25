using Data.Interfaces;

using Realms;

namespace Data.Entities
{
    public class ListeningMaterial : RealmObject, IEntityBase, IMaterial
    {
        [Required] public string Title { get; set; }
        [Required][PrimaryKey] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Text { get; set; }
        public byte[] Audio { get; set; }
        public byte[] Image { get; set; }
    }
}
