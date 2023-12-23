using Data.Interfaces;
using Data.Models;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class ReadingMaterialEntity : RealmObject, IEntityBase
    {
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;

        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; }
    }
}
