using Data.Entities;
using Data.Interfaces;

namespace Data.Models
{

    public class Segment : ModelBase, ISegment
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ReadingMaterial> ReadingMaterials { get;} 
        public List<ListeningMaterial> ListeningMaterials { get; }
        public CelebrityWordsQuiz CelebrityWodsQuiz { get; set; }

        public Segment(SegmentEntity segmentEntity) {
            Id = segmentEntity.Id;
            CreatedAt = segmentEntity.CreatedAt;
            ModifiedAt = segmentEntity.ModifiedAt;

            Title = segmentEntity.Title;
            Description = segmentEntity.Description;
            
            ReadingMaterials = new();
            foreach (var rmEntity in segmentEntity.ReadingMaterials) {
                ReadingMaterials.Add(new ReadingMaterial(rmEntity));
            }

            ListeningMaterials = new();
            foreach (var lmEntity in segmentEntity.ListeningMaterials) {
                ListeningMaterials.Add(new ListeningMaterial(lmEntity));
            }

            CelebrityWodsQuiz = new(segmentEntity.CelebrityWordsQuiz);
        }
    }
}
