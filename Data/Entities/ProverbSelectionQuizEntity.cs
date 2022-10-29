using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;

namespace Data.Entities
{
    public class ProverbSelectionQuizEntity : RealmObject, IEntityBase, IProverbSelectionQuiz
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
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string CorrectQuizId { get; set; }
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
