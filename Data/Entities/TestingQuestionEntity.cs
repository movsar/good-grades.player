using Data.Interfaces;
using Data.Models;
using Data.Services;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class TestingQuestionEntity : RealmObject, IEntityBase, ITestingQuestion
    {
        public TestingQuestionEntity() { }
        public TestingQuestionEntity(TestingQuestionEntity question)
        {
            Id = question.Id;
            CorrectQuizId = question.CorrectQuizId;
            QuestionText = question.QuestionText;
            QuizItems = new List<QuizItemEntity>(
                question.QuizItems.Select(qi => new QuizItemEntity(qi))
            );
        }
        #region Properties
        [Required]
        [PrimaryKey]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string QuestionText { get; set; }
        public string CorrectQuizId { get; set; }
        public IList<QuizItemEntity> QuizItems { get; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        #endregion

        #region HelperMethods
        public IModelBase ToModel()
        {
            return new TestingQuestion(this);
        }
        public void SetFromModel(IModelBase model)
        {
            var testingQuestion = model as TestingQuestion;
            QuestionText = testingQuestion.QuestionText;
            CorrectQuizId = testingQuestion.CorrectQuizId;

            Utils.SyncLists(QuizItems, testingQuestion.QuizItems);
        }
        #endregion
    }
}
