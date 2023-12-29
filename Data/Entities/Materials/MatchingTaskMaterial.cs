using Data.Entities.Materials.QuizItems;
using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities.Materials
{
    /*
     * This one allows to set an image and a text for each item, then it could be used for 
     * 2 types of tasks, matching image to text and text to image     
     */
    public class MatchingTaskMaterial : RealmObject, ITaskMaterial
    {
        [Required] public string Title { get; set; }
        [Required][PrimaryKey] public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public IList<TextAndImageQuizItem> Matches { get; }

        // If this is true, it means users will have to match the text to the image they see, instead of the other way around.
        public bool ReverseDirection { get; set; } = false;
    }
}
