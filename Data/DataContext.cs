using Data.Entities;
using Data.Entities.TaskItems;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext : DbContext
    {
        public string _dbPath = string.Empty;

        public DataContext() { }
        public DataContext(string filePath)
        {
            _dbPath = filePath;
        }

        public DbSet<DbMeta> DbMetas { get; set; }

        public DbSet<ReadingMaterial> ReadingMaterials { get; set; }
        public DbSet<ListeningMaterial> ListeningMaterials { get; set; }

        public DbSet<MatchingTaskAssignment> MatchingTaskAssignments { get; set; }
        public DbSet<FillingTaskAssignment> FillingTaskAssignments { get; set; }
        public DbSet<BuildingTaskAssignment> BuildingTaskAssignmeents { get; set; }
        public DbSet<SelectingTaskAssignment> SelectingTaskAssignments { get; set; }
        public DbSet<TestingTaskAssignment> TestingTaskAssignments { get; set; }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<Question> TestingQuestions { get; set; }
        public DbSet<AssignmentItem> AssignmentItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string connectionString = $"Data Source=" + _dbPath;
            options.UseSqlite(connectionString);
        }
    }
}
