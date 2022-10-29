using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class DbMeta : ModelBase, IDbMeta
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AppVersion { get; set; }

        public DbMeta() { }

        public DbMeta(DbMetaEntity optionEntity)
        {
            Id = optionEntity.Id;
            CreatedAt = optionEntity.CreatedAt;
            ModifiedAt = optionEntity.ModifiedAt;

            Title = optionEntity.Title;
            Description = optionEntity.Description;
            AppVersion = optionEntity.AppVersion;
        }
    }
}
