using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Microsoft.Extensions.Logging;
using Realms;

namespace Data
{
    public class Storage
    {
        public event Action<string> DatabaseInitialized;

        private Realm _realmInstance;
        private ILogger _logger;
        public SegmentRepository SegmentsRepository { get; private set; }
        public CwqRepository CwqRepository { get; private set; }
        public PsqRepository PsqRepository { get; private set; }
        public PbqRepository PbqRepository { get; private set; }
        public GfqRepository GfqRepository { get; private set; }
        public TsqRepository TsqRepository { get; private set; }
        public DbMetaRepository DbMetaRepository { get; private set; }
        public Storage(ILogger<Storage> logger) {
            _logger = logger;
        }

        public void OpenDatabase(string databasePath)
        {
            if (!InitializeDatabase(databasePath))
            {
                return;
            };

            DatabaseInitialized?.Invoke(databasePath);
        }

        public void CreateDatabase(string databasePath)
        {
            if (File.Exists(databasePath))
            {
                DropDatabase(databasePath);
            }

            if (!InitializeDatabase(databasePath)) {
                return;
            };

            var dbMeta = new DbMeta();
            DbMetaRepository.Add(ref dbMeta);

            DatabaseInitialized?.Invoke(databasePath);
        }

        private bool InitializeDatabase(string databasePath)
        {
            // Compacts the database if its size exceedes 30 MiB
            var dbConfig = new RealmConfiguration(databasePath)
            {
                ShouldCompactOnLaunch = (totalBytes, usedBytes) =>
                {
                    ulong edgeSize = 30 * 1024 * 1024;
                    return totalBytes > edgeSize && usedBytes / totalBytes < 0.5;
                }
            };
            try
            {
                _realmInstance = Realm.GetInstance(dbConfig);
            }catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex.Source, ex.StackTrace, ex.InnerException);
                return false;
            }
            InitializeRepositories();

            return true;
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