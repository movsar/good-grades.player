using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class SegmentRepository : GeneralRepository<SegmentEntity>
    {
        private Realm _realmInstance;
        public SegmentRepository(Realm realmInstance) : base(realmInstance)
        {
            _realmInstance = realmInstance;
        }
        public override IEnumerable<TModel> GetAll<TModel>()
        {
            var allSegments = base.GetAll<Segment>();
            return (IEnumerable<TModel>)allSegments;
        }
    }
}
