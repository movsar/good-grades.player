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

            Options = new();
            foreach (var optionEntity in celebrityWordsQuiz.Options)
            {
                Options.Add(new CwqOption(optionEntity));
            }
        }

        public List<CwqOption> Options { get; set; }
    }
}