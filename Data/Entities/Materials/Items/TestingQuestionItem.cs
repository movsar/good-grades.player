using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.Materials.QuizItems
{
    public class TestingQuestionItem : RealmObject, IEntityBase
    {
        [Required]
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string QuestionText { get; set; }
        public string CorrectAnswerId { get; set; }
        public IList<TextQuizItem> Answers { get; }
    }
}
