using Data.Entities.TaskItems;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class TestingTaskAssignment : RealmObject, ITaskAssignment
    {
        [Required] public string Title { get; set; }
        [Required][PrimaryKey] public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public IList<Question> Questions { get; }
        public bool IsContentSet => Questions.Count() > 0;
    }
}
