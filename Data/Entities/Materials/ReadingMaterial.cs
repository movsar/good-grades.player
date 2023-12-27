using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.Materials
{
    public class ReadingMaterial : RealmObject, IEntityBase
    {
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/

        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
    }
}
