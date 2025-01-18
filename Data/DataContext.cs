using Data.Entities;
using Data.Entities.TaskItems;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext : DbContext
    {
        public string DbPath { get; set; } = @"D:\source\movsar\good-grades.assets\Database\GoodGradesSqlite22.sgb";

        public DbSet<DbMeta> DbMetas { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<MatchingAssignment> MatchingTaskAssignments { get; set; }
        public DbSet<FillingAssignment> FillingTaskAssignments { get; set; }
        public DbSet<BuildingAssignment> BuildingTaskAssignmeents { get; set; }
        public DbSet<SelectingAssignment> SelectingTaskAssignments { get; set; }
        public DbSet<TestingAssignment> TestingTaskAssignments { get; set; }

        public DbSet<Segment> Segments { get; set; }
        public DbSet<Question> TestingQuestions { get; set; }
        public DbSet<AssignmentItem> AssignmentItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string connectionString = $"Data Source=" + DbPath;
            options.UseLazyLoadingProxies();
            options.UseSqlite(connectionString, options => options.MigrationsAssembly("Data"));

            base.OnConfiguring(options);
        }
    }
}
