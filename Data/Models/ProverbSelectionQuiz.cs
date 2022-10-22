using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class ProverbSelectionQuiz : ModelBase, IProverbSelectionQuiz
    {
        public ProverbSelectionQuiz(ProverbSelectionQuizEntity proverSelectionQuizEntity)
        {
            Id = proverSelectionQuizEntity.Id;
            CreatedAt = proverSelectionQuizEntity.CreatedAt;
            ModifiedAt = proverSelectionQuizEntity.ModifiedAt;

            Options = new();
            foreach (var optionEntity in proverSelectionQuizEntity.Options)
            {
                Options.Add(new QuizItem(optionEntity));
            }
        }

        public List<QuizItem> Options { get; set; }
    }
}