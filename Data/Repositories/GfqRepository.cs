using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class GfqRepository : GeneralRepository<GapFillerQuizEntity>
    {
        private readonly Realm _realmInstance;

        internal GfqRepository(Realm realm) : base(realm)
        {
            _realmInstance = realm;
        }

        public override void Update<TModel>(TModel model)
        {
            base.Update(model);
        }

    }
}
