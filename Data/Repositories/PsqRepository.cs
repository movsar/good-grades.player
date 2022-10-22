using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class PsqRepository : GeneralRepository<ProverbSelectionQuizEntity>
    {
        private readonly Realm _realmInstance;

        internal PsqRepository(Realm realm) : base(realm) {
            _realmInstance = realm;
        }

        public override void Update<TModel>(TModel model) {
            base.Update(model);
        }

    }
}
