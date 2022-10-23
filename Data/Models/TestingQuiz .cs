using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class TestingQuiz : ModelBase, IGapFillerQuiz
    {
        public TestingQuiz(TestingQuizEntity testingQuizEntity)
        {
            Id = testingQuizEntity.Id;
            CreatedAt = testingQuizEntity.CreatedAt;
            ModifiedAt = testingQuizEntity.ModifiedAt;

            Questions = new();
            foreach (var optionEntity in testingQuizEntity.Questions)
            {
                Questions.Add(new TestingQuestion(optionEntity));
            }
        }
        public List<TestingQuestion> Questions { get; set; }
    }
}