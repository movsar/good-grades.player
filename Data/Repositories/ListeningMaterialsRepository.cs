using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class ListeningMaterialsRepository : GeneralRepository<ListeningMaterialEntity>
    {
        private readonly Realm _realmInstance;

        internal ListeningMaterialsRepository(Realm realm) : base(realm)
        {
            _realmInstance = realm;
        }
    }
}
