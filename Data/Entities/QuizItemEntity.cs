using Data.Interfaces;
using Data.Models;
using MongoDB.Bson;
using Realms;

namespace Data.Entities {
    public class QuizItemEntity : RealmObject, IQuizItem, IEntityBase {
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public byte[] Image { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;

        public IModelBase ToModel() {
            return new QuizItem(this);
        }

        public void SetFromModel(IModelBase model) {
            var cwqOption = model as QuizItem;
            Image = cwqOption!.Image;
            Text = cwqOption!.Text;
        }
    }
}
