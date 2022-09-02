using Microsoft.EntityFrameworkCore;
using WPR.Common;

using System.IO;

namespace WPR.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Application>? Applications { get; set; }
        private static ApplicationContext? _Current;

        private const string DatabasePath = "Database\\applications.db";
        private const string DatabaseFolder = "Database\\";

        public static ApplicationContext Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new ApplicationContext();
                }

                return _Current;
            }
        }

        public ApplicationContext()
        {
            Directory.CreateDirectory(Configuration.Current.DataPath(DatabaseFolder));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .HasKey(c => c.Id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={Configuration.Current.DataPath(DatabasePath)}");
    }
}
