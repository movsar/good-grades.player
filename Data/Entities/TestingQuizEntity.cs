using Data.Interfaces;
using Data.Models;
using Data.Services;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class TestingQuizEntity : IEntityBase, ITestingQuiz
    {
        public TestingQuizEntity() { }
        public TestingQuizEntity(TestingQuizEntity testingQuiz)
        {
            Id = testingQuiz.Id;
            Questions = new List<TestingQuestionEntity>(
                testingQuiz.Questions.Select(q => new TestingQuestionEntity(q))
            );
        }
        #region Properties
        [Required]
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public IList<TestingQuestionEntity> Questions { get; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
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
