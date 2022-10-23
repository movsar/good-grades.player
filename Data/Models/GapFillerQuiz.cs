using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class GapFillerQuiz : ModelBase, IQuiz, IGapFillerQuiz
    {
        public GapFillerQuiz(GapFillerQuizEntity gapFillerQuizEntity)
        {
            Id = gapFillerQuizEntity.Id;
            CreatedAt = gapFillerQuizEntity.CreatedAt;
            ModifiedAt = gapFillerQuizEntity.ModifiedAt;

            QuizItems = new();
            foreach (var optionEntity in gapFillerQuizEntity.QuizItems)
            {
                QuizItems.Add(new QuizItem(optionEntity));
            }
        }
        public List<QuizItem> QuizItems { get; set; }
    }
}