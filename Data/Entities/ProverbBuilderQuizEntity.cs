using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;

namespace Data.Entities
{
    public class ProverbBuilderQuizEntity : RealmObject, IEntityBase, IProverbBuilderQuiz
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
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
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
