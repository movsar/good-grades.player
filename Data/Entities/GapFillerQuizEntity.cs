using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class GapFillerQuizEntity : RealmObject, IEntityBase
    {
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public IList<QuizItemEntity> QuizItems { get; }
        #endregion
    }
}
