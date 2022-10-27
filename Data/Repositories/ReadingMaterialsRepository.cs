using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class ReadingMaterialsRepository : GeneralRepository<ReadingMaterialEntity>
    {
        private readonly Realm _realmInstance;

        internal ReadingMaterialsRepository(Realm realm) : base(realm)
        {
            _realmInstance = realm;
        }
    }
}
