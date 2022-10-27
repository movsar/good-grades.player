using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class QuizItemsRepository : GeneralRepository<QuizItemEntity>
    {
        private readonly Realm _realmInstance;

        internal QuizItemsRepository(Realm realm) : base(realm)
        {
            _realmInstance = realm;
        }
    }
}
