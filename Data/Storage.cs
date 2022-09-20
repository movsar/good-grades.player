using Data.Repositories;
using Realms;

namespace Data
{
    public class Storage
    {
        private readonly Realm _realmInstance;
        public SegmentRepository SegmentsRepository { get; }
        public Storage(bool cleanStart = false, string databasePath = "content.sgb")
        {
            RealmConfiguration DbConfiguration = new(databasePath);

            if (cleanStart)
            {
                Realm.DeleteRealm(DbConfiguration);
            }

            _realmInstance = Realm.GetInstance(DbConfiguration);

            SegmentsRepository = new SegmentRepository(_realmInstance);
        }

        public void DropDatabase(string dbPath)
        {
            Realm.DeleteRealm(new RealmConfiguration(dbPath));
        }
    }
}