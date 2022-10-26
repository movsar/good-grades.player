using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Realms;

namespace Data
{
    public class Storage
    {
        public event Action<string> DatabaseInitialized;

        private Realm _realmInstance;
        public SegmentRepository SegmentsRepository { get; private set; }
        public CwqRepository CwqRepository { get; private set; }
        public PsqRepository PsqRepository { get; private set; }
        public PbqRepository PbqRepository { get; private set; }
        public GfqRepository GfqRepository { get; private set; }
        public TsqRepository TsqRepository { get; private set; }
        public DbMetaRepository DbMetaRepository { get; private set; }
        public Storage() { }

        public void OpenDatabase(string databasePath)
        {
            InitializeDatabase(databasePath);

            DatabaseInitialized?.Invoke(databasePath);
        }

        public void CreateDatabase(string databasePath)
        {
            if (File.Exists(databasePath))
            {
                DropDatabase(databasePath);
            }

            InitializeDatabase(databasePath);

            var dbMeta = new DbMeta();
            DbMetaRepository.Add(ref dbMeta);

            DatabaseInitialized?.Invoke(databasePath);
        }

        private void InitializeDatabase(string databasePath)
        {
            RealmConfiguration DbConfiguration = new(databasePath);
            _realmInstance = Realm.GetInstance(DbConfiguration);

            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            SegmentsRepository = new SegmentRepository(_realmInstance);
            CwqRepository = new CwqRepository(_realmInstance);
            PsqRepository = new PsqRepository(_realmInstance);
            PbqRepository = new PbqRepository(_realmInstance);
            GfqRepository = new GfqRepository(_realmInstance);
            TsqRepository = new TsqRepository(_realmInstance);
            DbMetaRepository = new DbMetaRepository(_realmInstance);
        }

        public void DropDatabase(string dbPath)
        {
            try
            {
                Realm.DeleteRealm(new RealmConfiguration(dbPath));
                File.Delete(dbPath);
            }
            catch (Exception)
            {
                // Log error
            }
        }
    }
}