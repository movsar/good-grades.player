using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.TaskItems
{
    public class TestingQuestion : RealmObject, IEntityBase
    {
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string QuestionText { get; set; }
        public string CorrectAnswerId { get; set; }
        public IList<TextItem> Answers { get; }
    }
}
