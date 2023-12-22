using Data.Interfaces;
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class DbMetaEntity : IEntityBase, IDbMeta
    {
        #region Properties
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? AppVersion { get; set; }
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new DbMeta(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var dbMeta = model as DbMeta;
            Title = dbMeta!.Title;
            Description = dbMeta.Description;
            AppVersion = dbMeta.AppVersion;
        }
        #endregion

    }
}