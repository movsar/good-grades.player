using Data.Entities.Materials.QuizItems;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.Materials
{
    public class BuildingTaskMaterial : RealmObject, IEntityBase
    {
        [Required] [PrimaryKey] public string Id { get; } = ObjectId.GenerateNewId().ToString();
        [Required] public string Title { get; set; }
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public IList<TextQuizItem> Words { get; }
    }
}
