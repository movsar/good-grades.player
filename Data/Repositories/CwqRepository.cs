using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class CwqRepository : GeneralRepository<TextToImageQuizEntity>
    {
        private readonly Realm _realmInstance;

        internal CwqRepository(Realm realm) : base(realm) {
            _realmInstance = realm;
        }

        public override void Update<TModel>(TModel model) {
            base.Update(model);
        }

        internal void DeleteBySegmentId(string segmentId) {
            // Is this really needed?
            //var entries = _realmInstance.All<CelebrityWordsQuizEntity>().Where(cwq => cwq.SegmentId == segmentId);
            //_realmInstance.Write(() => {
            //    _realmInstance.Remove(entries);
            //});
        }
    }
}
