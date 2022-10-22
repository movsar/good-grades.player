using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class ProverbSelectionQuiz : ModelBase, IQuiz, IProverbSelectionQuiz
    {
        public ProverbSelectionQuiz(ProverbSelectionQuizEntity proverSelectionQuizEntity)
        {
            Id = proverSelectionQuizEntity.Id;
            CreatedAt = proverSelectionQuizEntity.CreatedAt;
            ModifiedAt = proverSelectionQuizEntity.ModifiedAt;

            QuizItems = new();
            foreach (var optionEntity in proverSelectionQuizEntity.Options)
            {
                QuizItems.Add(new QuizItem(optionEntity));
            }
        }

        public List<QuizItem> QuizItems { get; set; }
    }
}