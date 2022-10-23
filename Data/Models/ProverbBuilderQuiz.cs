using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class ProverbBuilderQuiz : ModelBase, IProverbBuilderQuiz
    {
        public ProverbBuilderQuiz(ProverbBuilderQuizEntity proverbBuilderQuizEntity)
        {
            Id = proverbBuilderQuizEntity.Id;
            CreatedAt = proverbBuilderQuizEntity.CreatedAt;
            ModifiedAt = proverbBuilderQuizEntity.ModifiedAt;

            QuizItems = new();
            foreach (var optionEntity in proverbBuilderQuizEntity.QuizItems)
            {
                QuizItems.Add(new QuizItem(optionEntity));
            }
        }
        public List<QuizItem> QuizItems { get; set; }
    }
}