using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class CwqRepository : GeneralRepository<CelebrityWordsQuizEntity>
    {
        private readonly Realm _realmInstance;

        internal CwqRepository(Realm realm) : base(realm) {
            _realmInstance = realm;
        }
        
        public IEnumerable<CelebrityWordsQuiz> GetBySegmentId(string segmentId) {
            var entries = _realmInstance.All<CelebrityWordsQuizEntity>().Where(cwq => cwq.SegmentId == segmentId);
            var models = EntitiesToModels<CelebrityWordsQuizEntity, CelebrityWordsQuiz>(entries);
            return models;
        }

        public override void Update<TModel>(TModel model) {
            base.Update(model);
        }

        internal void DeleteBySegmentId(string segmentId) {
            var entries = _realmInstance.All<CelebrityWordsQuizEntity>().Where(cwq => cwq.SegmentId == segmentId).First();
            _realmInstance.Write(() => {
                _realmInstance.Remove(entries);
            });
        }
    }
}
