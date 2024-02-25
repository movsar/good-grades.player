using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class DbMeta : RealmObject, IEntityBase
    {
        [PrimaryKey] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AppVersion { get; set; }

    }
}