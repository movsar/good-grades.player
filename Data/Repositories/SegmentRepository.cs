using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories {
    public class SegmentRepository : GeneralRepository<SegmentEntity> {
        private Realm _realmInstance;
        public SegmentRepository(Realm realmInstance) : base(realmInstance) {
            _realmInstance = realmInstance;
        }
        public override void Delete<TModel>(TModel model) {
            var cwqRepository = new CwqRepository(_realmInstance);
            cwqRepository.DeleteBySegmentId(model.Id);

            base.Delete(model);
        }
        public override IEnumerable<TModel> GetAll<TModel>() {
            var allSegments = base.GetAll<Segment>();

            // Associate CelebrityWordsQuiz objects with Segment objects
            var cwqRepository = new CwqRepository(_realmInstance);
            var allCwqs = cwqRepository.GetAll<CelebrityWordsQuiz>();

            foreach (var segment in allSegments) {
                segment.CelebrityWodsQuiz = allCwqs.First(cwq => cwq.SegmentId == segment.Id);
            }

            return (IEnumerable<TModel>)allSegments;
        }
    }
}
