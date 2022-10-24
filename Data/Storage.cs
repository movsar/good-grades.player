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
        private readonly bool _firstStart = false;
        public SegmentRepository SegmentsRepository { get; }
        public CwqRepository CwqRepository { get; }
        public PsqRepository PsqRepository { get; }
        public PbqRepository PbqRepository { get; }
        public GfqRepository GfqRepository { get; }
        public TsqRepository TsqRepository { get; }
        public DbMetaRepository DbMetaRepository { get; }

        public Storage(bool cleanStart = false, string databasePath = "content.sgb")
        {
            var sameDirPath = Path.Combine(Environment.CurrentDirectory, databasePath);

            RealmConfiguration DbConfiguration = new(databasePath);

            if (cleanStart)
            {
                Realm.DeleteRealm(DbConfiguration);
            }

            // First start flag
            if (!File.Exists(DbConfiguration.DatabasePath))
            {
                _firstStart = true;
            }

            // Initialize database and repositories
            _realmInstance = Realm.GetInstance(DbConfiguration);
            SegmentsRepository = new SegmentRepository(_realmInstance);
            CwqRepository = new CwqRepository(_realmInstance);
            PsqRepository = new PsqRepository(_realmInstance);
            PbqRepository = new PbqRepository(_realmInstance);
            GfqRepository = new GfqRepository(_realmInstance);
            TsqRepository = new TsqRepository(_realmInstance);
            DbMetaRepository = new DbMetaRepository(_realmInstance);

            // Add database metadata object on first start
            if (_firstStart)
            {
                var dbMeta = new DbMeta();
                DbMetaRepository.Add(ref dbMeta);
            }
        }

        public void DropDatabase(string dbPath)
        {
            Realm.DeleteRealm(new RealmConfiguration(dbPath));
        }
    }
}