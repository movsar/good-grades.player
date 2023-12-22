using Data.Interfaces;
using Data.Models;
using Data.Services;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ProverbBuilderQuizEntity : IEntityBase, IProverbBuilderQuiz
    {
        public ProverbBuilderQuizEntity() { }
        public ProverbBuilderQuizEntity(ProverbBuilderQuizEntity proverbBuilderQuiz)
        {
            Id = proverbBuilderQuiz.Id;
            QuizItems = new List<QuizItemEntity>(
              proverbBuilderQuiz.QuizItems.Select(qi => new QuizItemEntity(qi))
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
            return new ProverbBuilderQuiz(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var proverbBuilderQuiz = model as ProverbBuilderQuiz;
            Utils.SyncLists(QuizItems, proverbBuilderQuiz.QuizItems);
        }
        #endregion
    }
}
