using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;

namespace Data.Entities
{
    public class CelebrityWordsQuizEntity : RealmObject, IEntityBase, ICelebrityWordsQuiz
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
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
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
