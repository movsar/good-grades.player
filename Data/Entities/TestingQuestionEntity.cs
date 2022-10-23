using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;

namespace Data.Entities
{
    public class TestingQuestionEntity : RealmObject, IEntityBase, ITestingQuestion
    {
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public IList<QuizItemEntity> QuizItems { get; }
        public string Question { get; set; }
        public string AnswerId { get; set; }
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new TestingQuestion(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var testingQuestion = model as TestingQuestion;
            Question = testingQuestion.Question;
            AnswerId = testingQuestion.AnswerId;
            Utils.SyncLists(QuizItems, testingQuestion.QuizItems);
        }
        #endregion
    }
}
