using Data.Entities.TaskItems;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class SelectingTaskAssignment : RealmObject, ITaskAssignment
    {
        [Required] public string Title { get; set; }
        [Required][PrimaryKey] public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string CorrectItemId { get; set; }
        public IList<AssignmentItem> Items { get; }
        public bool IsContentSet => Items.Count() > 0 && !string.IsNullOrEmpty(CorrectItemId);

    }
}
