using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;

namespace Data.Entities
{
    public class GapFillerQuizEntity : RealmObject, IEntityBase, IGapFillerQuiz
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
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
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
