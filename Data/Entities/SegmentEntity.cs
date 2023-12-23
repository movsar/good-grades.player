using Data.Interfaces;
using MongoDB.Bson;
using Realms;

namespace Data.Entities
{
    public class SegmentEntity : RealmObject, IEntityBase
    {
        [PrimaryKey]
        public string Id { get; } = ObjectId.GenerateNewId().ToString();
        public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        public IList<TextToImageQuizEntity> TextToImageQuizes { get; }
        public IList<ProverbSelectionQuizEntity> ProverbSelectionQuizes { get; }
        public IList<ProverbBuilderQuizEntity> ProverbBuilderQuizes { get; }
        public IList<GapFillerQuizEntity> GapFillerQuizes { get; }
        public IList<TestingQuizEntity> TestingQuizes { get; }
        public IList<ReadingMaterialEntity> ReadingMaterials { get; }
        public IList<ListeningMaterialEntity> ListeningMaterials { get; }

    }
}
