using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{
    public class CelebrityWordsQuiz : ModelBase, ICelebrityWordsQuiz
    {
        public CelebrityWordsQuiz(CelebrityWordsQuizEntity celebrityWordsQuiz)
        {
            Id = celebrityWordsQuiz.Id;
            CreatedAt = celebrityWordsQuiz.CreatedAt;
            ModifiedAt = celebrityWordsQuiz.ModifiedAt;

            QuizItems = new();
            foreach (var optionEntity in celebrityWordsQuiz.QuizItems)
            {
                QuizItems.Add(new QuizItem(optionEntity));
            }
        }

        public List<QuizItem> QuizItems { get; set; }
    }
}