using Data.Entities;
using Data.Interfaces;

namespace Data.Models {
    public class CelebrityWordsQuiz : ModelBase, ICelebrityWordsQuiz {
        public CelebrityWordsQuiz(CelebrityWordsQuizEntity celebrityWordsQuiz) {
            Id = celebrityWordsQuiz.Id;
            CreatedAt = celebrityWordsQuiz.CreatedAt;
            ModifiedAt = celebrityWordsQuiz.ModifiedAt;

            SegmentId = celebrityWordsQuiz.SegmentId;
            Options = new();
            foreach (var optionEntity in celebrityWordsQuiz.Options) {
                Options.Add(new CwqOption(optionEntity));
            }
        }

        public string SegmentId { get; set; }
        public List<CwqOption> Options { get; set; }
    }
}