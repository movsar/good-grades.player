using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class Storage
    {
        public event Action<string> DatabaseInitialized;
        public event Action DatabaseUpdated;

        private ILogger _logger;
        public DbMetaRepository DbMetaRepository { get; private set; }
        public SegmentRepository SegmentsRepository { get; private set; }

        public ReadingMaterialsRepository ReadingMaterialsRepository { get; private set; }
        public ListeningMaterialsRepository ListeningMaterialsRepository { get; private set; }
        public QuizItemsRepository QuizItemsRepository { get; private set; }
        public QuestionsRepository QuestionsRepository { get; private set; }

        public CwqRepository CwqRepository { get; private set; }
        public PsqRepository PsqRepository { get; private set; }
        public PbqRepository PbqRepository { get; private set; }
        public GfqRepository GfqRepository { get; private set; }
        public TsqRepository TsqRepository { get; private set; }
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

            var dbMeta = new DbMeta()
            {
                Title = Path.GetFileNameWithoutExtension(databasePath),
                AppVersion = appVersion
            };
            DbMetaRepository.Add(ref dbMeta);

            DatabaseInitialized?.Invoke(databasePath);
        }

        public static DataContext GetDataContext()
        {
            return new DataContext();
        }

        private bool InitializeDatabase(string databasePath)
        {
            // Compacts the database if its size exceedes 30 MiB
           
            try
            {
                
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex.Source, ex.StackTrace, ex.InnerException);
                return false;
            }
            InitializeRepositories();

            return true;
        }

        private void InitializeRepositories()
        {
            SegmentsRepository = new SegmentRepository();

            CwqRepository = new CwqRepository();
            PsqRepository = new PsqRepository();
            PbqRepository = new PbqRepository();
            GfqRepository = new GfqRepository();
            TsqRepository = new TsqRepository();

            ReadingMaterialsRepository = new ReadingMaterialsRepository();
            ListeningMaterialsRepository = new ListeningMaterialsRepository();
            QuizItemsRepository = new QuizItemsRepository();
            QuestionsRepository = new QuestionsRepository();
            DbMetaRepository = new DbMetaRepository();
        }

        public void DropDatabase(string dbPath)
        {
            try
            {
                File.Delete(DataContext.DB_PATH);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            }
        }

        public void ImportDatabase(string filePath)
        {
            //var realmToImport = Realm.GetInstance(filePath);

            //var segments = realmToImport.All<SegmentEntity>();

            //try
            //{
            //    _realmInstance.Write(() =>
            //    {
            //        foreach (SegmentEntity segment in segments)
            //        {
            //            _realmInstance.Add(new SegmentEntity(segment), true);
            //        }
            //    });

            //    DatabaseUpdated?.Invoke();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message, ex.StackTrace, ex.InnerException);
            //    throw;
            //}
        }
    }
}
