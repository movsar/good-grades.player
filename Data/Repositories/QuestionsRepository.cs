using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Realms;

namespace Data.Repositories
{
    public class QuestionsRepository : GeneralRepository<TestingQuestionEntity>
    {
        private readonly Realm _realmInstance;

        internal QuestionsRepository(Realm realm) : base(realm)
        {
            _realmInstance = realm;
        }
    }
}
