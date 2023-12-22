using Data.Entities;
namespace Data.Repositories
{
    public class CwqRepository : GeneralRepository<CelebrityWordsQuizEntity>
    {
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
