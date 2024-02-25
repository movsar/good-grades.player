using Data.Entities.TaskItems;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class SelectingTaskAssignment : RealmObject, IAssignment
    {
        [Required] public string Title { get; set; }
        [Required][PrimaryKey] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public Question Question { get; set; } = new Question();
        public bool IsContentSet => Question.Options.Count() > 0 && !string.IsNullOrEmpty(Question.CorrectOptionId);
    }
}
