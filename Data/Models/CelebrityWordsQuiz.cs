using Data.Interfaces;

namespace Data.Models {
    public class CelebrityWordsQuiz : ModelBase, ICelebrityWordsQuiz {
        public string SegmentId { get; set; }
        public Dictionary<string, KeyValuePair<byte[], string>> Data { get; set; } = new();
        public CelebrityWordsQuiz(string segmentId) {
            SegmentId = segmentId;
        }
        public CelebrityWordsQuiz() { }
    }
}