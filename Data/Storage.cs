using Data.Entities;
using Microsoft.Extensions.Logging;
using Realms;

namespace Data
{
    public class Storage
    {
        public Realm Database
        {
            get
            {
                try
                {
                    return Realm.GetInstance(_dbConfig);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message, ex.Source, ex.StackTrace, ex.InnerException);
                    throw;
                }
            }
        }

        private ILogger _logger;
        private RealmConfiguration _dbConfig;
        public Storage(ILogger<Storage> logger)
        {
            _logger = logger;
        }

        public void SetDatabaseConfig(string databasePath)
        {
            // Compacts the database if its size exceedes 30 MiB
            _dbConfig = new RealmConfiguration(databasePath)
            {
                ShouldCompactOnLaunch = (totalBytes, usedBytes) =>
                {
                    ulong edgeSize = 30 * 1024 * 1024;
                    return totalBytes > edgeSize && usedBytes / totalBytes < 0.5;
                }
            };
        }
        public void CreateDatabase(string databasePath, string? appVersion)
        {
            if (File.Exists(databasePath))
            {
                DropDatabase(databasePath);
            }

            SetDatabaseConfig(databasePath);

            var dbMeta = new DbMeta()
            {
                Title = Path.GetFileNameWithoutExtension(databasePath),
                AppVersion = appVersion
            };

            Database.Write(() => Database.Add(dbMeta));
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
                Database.Write(() =>
                {
                    foreach (SegmentEntity segment in segments)
                    {
                        Database.Add(segment, true);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
                throw;
            }
        }
    }
}
