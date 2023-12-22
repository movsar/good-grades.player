using Data.Entities;
using Data.Models;

namespace Data.Repositories
{
    public class SegmentRepository : GeneralRepository<SegmentEntity>
    {
        public override IEnumerable<TModel> GetAll<TModel>()
        {
            var allSegments = base.GetAll<Segment>();
            return (IEnumerable<TModel>)allSegments;
        }
    }
}
