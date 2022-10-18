using Data.Interfaces;

namespace Data.Models
{

    public class Segment : ModelBase, ISegment
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ReadingMaterial> ReadingMaterials { get; set; }
        public List<ListeningMaterial> ListeningMaterials { get; set; }
        public CelebrityWordsQuiz CelebrityWodsQuiz { get; set; }
    }
}
