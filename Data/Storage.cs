using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class Storage
    {
        public event Action<string> DatabaseInitialized;
        public event Action DatabaseUpdated;

        private ILogger _logger;
        public Storage(ILogger<Storage> logger)
        {
            _logger = logger;
        }
        public GenericRepository<DbMetaEntity> DbMetaRepository = new GenericRepository<DbMetaEntity>();
        public GenericRepository<SegmentEntity> SegmentsRepository => new GenericRepository<SegmentEntity>();
        public GenericRepository<ReadingMaterialEntity> ReadingMaterialsRepository => new GenericRepository<ReadingMaterialEntity>();
        public GenericRepository<ListeningMaterialEntity> ListeningMaterialsRepository => new GenericRepository<ListeningMaterialEntity>();
        public GenericRepository<QuizItemEntity> QuizItemsRepository => new GenericRepository<QuizItemEntity>();
        public GenericRepository<TestingQuestionEntity> TestingQuestionsRepository => new GenericRepository<TestingQuestionEntity>();
        public GenericRepository<CelebrityWordsQuizEntity> CwqRepository => new GenericRepository<CelebrityWordsQuizEntity>();
        public GenericRepository<ProverbSelectionQuizEntity> PsqRepository => new GenericRepository<ProverbSelectionQuizEntity>();
        public GenericRepository<ProverbBuilderQuizEntity> PbqRepository => new GenericRepository<ProverbBuilderQuizEntity>();
        public GenericRepository<GapFillerQuizEntity> GfqRepository => new GenericRepository<GapFillerQuizEntity>();
        public GenericRepository<TestingQuizEntity> TsqRepository => new GenericRepository<TestingQuizEntity>();
        public void OpenDatabase(string databasePath)
        {
            if (!InitializeDatabase(databasePath))
            {
                return;
            }

            DatabaseInitialized?.Invoke(databasePath);
        }
        private bool InitializeDatabase(string databasePath)
        {
            try
            {
                DataContext.DB_PATH = databasePath;
                using (var context = new DataContext())
                {
                    context.Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex.Source, ex.StackTrace, ex.InnerException);
                throw;
                //return false;
            }

            return true;
        }
        public void CreateDatabase(string databasePath, string? appVersion)
        {
            if (File.Exists(databasePath))
            {
                DropDatabase(databasePath);
            }
            InitializeDatabase(databasePath);

            var dbMeta = new DbMeta()
            {
                Title = Path.GetFileNameWithoutExtension(databasePath),
                AppVersion = appVersion
            };

            var dbMetaRepository = new GenericRepository<DbMetaEntity>();
            dbMetaRepository.Add(ref dbMeta);

            DatabaseInitialized?.Invoke(databasePath);
        }

        public static DataContext GetDataContext()
        {
            return new DataContext();
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
