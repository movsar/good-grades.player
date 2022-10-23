using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class TestingQuestion : ModelBase, ITestingQuestion
    {
        public TestingQuestion() { }
        public TestingQuestion(TestingQuestionEntity questionEntity)
        {
            Id = questionEntity.Id;
            CreatedAt = questionEntity.CreatedAt;
            ModifiedAt = questionEntity.ModifiedAt;

            QuizItems = new();
            foreach (var optionEntity in questionEntity.QuizItems)
            {
                QuizItems.Add(new QuizItem(optionEntity));
            }

            Question = questionEntity.Question;
            AnswerId = questionEntity.AnswerId;
        }
        public string Question { get; set; }
        public string AnswerId { get; set; }
        public List<QuizItem> QuizItems { get; set; }
    }
}