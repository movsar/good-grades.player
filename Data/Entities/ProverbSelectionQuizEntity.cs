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
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public IList<QuizItemEntity> Options { get; }
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new ProverbSelectionQuiz(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var ProverbSelectionQuiz = model as ProverbSelectionQuiz;
            Utils.SyncLists(Options, ProverbSelectionQuiz.Options);
        }
        #endregion
    }
}
