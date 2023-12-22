using Data.Interfaces;
using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities {
    public class QuizItemEntity : IQuizItem, IEntityBase {
        public QuizItemEntity() { }
        public QuizItemEntity(QuizItemEntity option)
        {
            Id = option.Id;
            Image = option.Image;
            Text = option.Text;
        }

        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
