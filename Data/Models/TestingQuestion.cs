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

            QuestionText = questionEntity.Question;
            AnswerId = questionEntity.AnswerId;
        }

        public TestingQuestion(string text, List<QuizItem> quizItems)
        {
            QuestionText = text;
            QuizItems = quizItems;
        }

        public string QuestionText { get; set; }
        public string AnswerId { get; set; }
        public List<QuizItem> QuizItems { get; set; } = new List<QuizItem>();
    }
}