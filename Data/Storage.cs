using Data.Entities;
using Microsoft.Extensions.Logging;
using Realms;

namespace Data
{
    public class Storage
    {
        public event Action<string> DatabaseInitialized;
        public event Action DatabaseUpdated;

        private Realm _realmInstance;
        private ILogger _logger;

        public RealmConfiguration DbConfig { get; private set; }

        public Storage(ILogger<Storage> logger)
        {
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

        public void CreateDatabase(string databasePath, string? appVersion)
        {
            if (File.Exists(databasePath))
            {
                DropDatabase(databasePath);
            }

            if (!InitializeDatabase(databasePath))
            {
                return;
            };

            var dbMeta = new DbMetaEntity()
            {
                Title = Path.GetFileNameWithoutExtension(databasePath),
                AppVersion = appVersion
            };

            _realmInstance.Write(() => _realmInstance.Add(dbMeta));

            DatabaseInitialized?.Invoke(databasePath);
        }

        private bool InitializeDatabase(string databasePath)
        {
            // Compacts the database if its size exceedes 30 MiB
            DbConfig = new RealmConfiguration(databasePath)
            {
                ShouldCompactOnLaunch = (totalBytes, usedBytes) =>
                {
                    ulong edgeSize = 30 * 1024 * 1024;
                    return totalBytes > edgeSize && usedBytes / totalBytes < 0.5;
                }
            };
            try
            {
                _realmInstance = Realm.GetInstance(DbConfig);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex.Source, ex.StackTrace, ex.InnerException);
                return false;
            }

            return true;
        }

        public void DropDatabase(string dbPath)
        {
            try
            {
                Realm.DeleteRealm(new RealmConfiguration(dbPath));
                File.Delete(dbPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        public void ImportDatabase(string filePath)
        {
            var realmToImport = Realm.GetInstance(filePath);

            var segments = realmToImport.All<SegmentEntity>();

            try
            {
                _realmInstance.Write(() =>
                {
                    foreach (SegmentEntity segment in segments)
                    {
                        _realmInstance.Add(segment, true);
                    }
                });

                DatabaseUpdated?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
                throw;
            }
        }
    }
}
