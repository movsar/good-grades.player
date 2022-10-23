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
            
            CorrectProverbId = proverSelectionQuizEntity.CorrectProverbId;

            QuizItems = new();
            foreach (var optionEntity in proverSelectionQuizEntity.QuizItems)
            {
                QuizItems.Add(new QuizItem(optionEntity));
            }
        }
        public string CorrectProverbId { get; set; }
        public List<QuizItem> QuizItems { get; set; }
    }
}