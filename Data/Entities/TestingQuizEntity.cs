using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;
using System.Collections.Generic;

namespace Data.Entities
{
    public class TestingQuizEntity : RealmObject, IEntityBase, ITestingQuiz
    {
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        public IList<TestingQuestionEntity> Questions { get; }
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new TestingQuiz(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var testingQuestion = model as TestingQuiz;
            Utils.SyncLists(Questions, testingQuestion.Questions);
        }
        #endregion
    }
}
