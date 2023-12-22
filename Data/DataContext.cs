using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
namespace Data
{
    public class DataContext : DbContext
    {
        public static string DB_PATH = string.Empty;
        public DbSet<CelebrityWordsQuizEntity> CelebrityWordsQuizes { get; set; }
        public DbSet<DbMetaEntity> DbMetas { get; set; }
        public DbSet<GapFillerQuizEntity> GapFillerQuizes { get; set; }
        public DbSet<ListeningMaterialEntity> ListeningMaterials { get; set; }
        public DbSet<ProverbBuilderQuizEntity> ProverbBuilderQuizes { get; set; }
        public DbSet<ProverbSelectionQuizEntity> ProverbSelectionQuizes { get; set; }
        public DbSet<QuizItemEntity> QuizItems { get; set; }
        public DbSet<ReadingMaterialEntity> ReadingMaterials { get; set; }
        public DbSet<SegmentEntity> Segments { get; set; }
        public DbSet<TestingQuestionEntity> TestingQuestions { get; set; }
        public DbSet<TestingQuizEntity> TestingQuizItems { get; set; }     
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string connectionString = $"Data Source=" + DB_PATH;
            options.UseSqlite(connectionString);
        }
    }
}
