using Data.Interfaces;
using Data.Models;
using Data.Services;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ProverbSelectionQuizEntity : IEntityBase, IProverbSelectionQuiz
    {
        public ProverbSelectionQuizEntity() { }
        public ProverbSelectionQuizEntity(ProverbSelectionQuizEntity proverbSelectionQuiz)
        {
            Id = proverbSelectionQuiz.Id;
            CorrectQuizId = proverbSelectionQuiz.CorrectQuizId;
            QuizItems = new List<QuizItemEntity>(
                proverbSelectionQuiz.QuizItems.Select(qi => new QuizItemEntity(qi))
            );
        }
        #region Properties
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CorrectQuizId { get; set; }
        public IList<QuizItemEntity> QuizItems { get; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new ProverbSelectionQuiz(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var ProverbSelectionQuiz = model as ProverbSelectionQuiz;
            CorrectQuizId = ProverbSelectionQuiz!.CorrectQuizId;
            Utils.SyncLists(QuizItems, ProverbSelectionQuiz.QuizItems);
        }
        #endregion
    }
}
