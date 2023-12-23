using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class DbMetaEntity : RealmObject, IEntityBase
    {
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AppVersion { get; set; }
        #endregion

    }
}