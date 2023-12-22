using Data.Interfaces;
using Data.Models;
using Data.Services;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class CelebrityWordsQuizEntity : IEntityBase, ICelebrityWordsQuiz
    {
        public CelebrityWordsQuizEntity() { }
        public CelebrityWordsQuizEntity(CelebrityWordsQuizEntity cwq)
        {
            Id = cwq.Id;
            QuizItems = new List<QuizItemEntity>(
                cwq.QuizItems.Select(qi => new QuizItemEntity(qi))
            );
            
            //foreach (var option in cwq.QuizItems)
            //{
            //    QuizItems.Add(new QuizItemEntity(option));
            //}
        }
        #region Properties
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public IList<QuizItemEntity> QuizItems { get; }
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new CelebrityWordsQuiz(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var celebrityWordsQuiz = model as CelebrityWordsQuiz;
            Utils.SyncLists(QuizItems, celebrityWordsQuiz!.QuizItems);
        }
        #endregion
    }
}
