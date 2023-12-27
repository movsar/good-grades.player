using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.Materials.QuizItems
{
    public class TextAndImageQuizItem : RealmObject, IEntityBase
    {
        [Required]
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public byte[] Image { get; set; }
        public string Text { get; set; }
    }
}
