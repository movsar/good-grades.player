using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;
using System.Reflection;

namespace Data.Entities
{
    public class DbMetaEntity : RealmObject, IEntityBase, IDbMeta
    {
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public string Title { get; set; }
        public string Description { get; set; }
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