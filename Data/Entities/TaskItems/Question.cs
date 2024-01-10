using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.TaskItems
{
    public class Question : RealmObject, IEntityBase
    {
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Text { get; set; }
        public string CorrectOptionId { get; set; }
        public IList<AssignmentItem> Options { get; }
    }
}
