using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.Materials.TaskItems
{
    public class TextItemEntity : RealmObject, IEntityBase
    {
        [Required]
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Text { get; set; }
    }
}
