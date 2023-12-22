using Data.Interfaces;
using Data.Models;
using Data.Services;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class GapFillerQuizEntity : IEntityBase, IGapFillerQuiz
    {
        public GapFillerQuizEntity() { }
        public GapFillerQuizEntity(GapFillerQuizEntity gapFillerQuiz)
        {
            Id = gapFillerQuiz.Id;
            QuizItems = new List<QuizItemEntity>(
               gapFillerQuiz.QuizItems.Select(qi => new QuizItemEntity(qi))
            );
        }
        #region Properties
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public IList<QuizItemEntity> QuizItems { get; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new GapFillerQuiz(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var gapFillerQuiz = model as GapFillerQuiz;
            Utils.SyncLists(QuizItems, gapFillerQuiz.QuizItems);
        }
        #endregion
    }
}
