using Data.Interfaces;

namespace Data.Models {
    public class CelebrityWordsQuiz : ModelBase, ICelebrityWordsQuiz {
        public string SegmentId { get; set; }
        public IList<CwqOption> Options { get; set; } = new List<CwqOption>();
        public CelebrityWordsQuiz(string segmentId) {
            SegmentId = segmentId;
        }
        public CelebrityWordsQuiz() { }
    }
}