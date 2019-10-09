using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using NewStackPlayground.Application.Domain;

namespace NewStackPlayground.Application.DataAccess
{
    public class EfContext : DbContext
    {
        private readonly DbConnection _dbConnection;

        public EfContext(
            DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public DbSet<TEntity> GetSet<TEntity>()
            where TEntity : class
            => Set<TEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder
                .UseNpgsql(_dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(x => x.Name).IsRequired();
            });
        }
    }
}
