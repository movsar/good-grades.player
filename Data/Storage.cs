using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Realms;

namespace Data
{
    public class Storage
    {
        private readonly Realm _realmInstance;
        public SegmentRepository SegmentsRepository { get; }
        public CwqRepository CwqRepository { get; }

        public Storage(bool cleanStart = false, string databasePath = "content.sgb")
        {
            var sameDirPath = Path.Combine(Environment.CurrentDirectory, databasePath);

            RealmConfiguration DbConfiguration = new(databasePath);

            if (cleanStart)
            {
                Realm.DeleteRealm(DbConfiguration);
            }

            _realmInstance = Realm.GetInstance(DbConfiguration);

            SegmentsRepository = new SegmentRepository(_realmInstance);
            CwqRepository = new CwqRepository(_realmInstance);
        }

        public void DropDatabase(string dbPath)
        {
            Realm.DeleteRealm(new RealmConfiguration(dbPath));
        }
    }
}