using Catalog.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Game> Game { get; set; }
        public DbSet<Member> Member { get; set; }
    }
}
