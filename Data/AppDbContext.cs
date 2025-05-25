using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using CosmoTest.Models;

namespace CosmoTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CosmoTest.Models.User> Users => Set<CosmoTest.Models.User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CosmoTest.Models.User>().ToContainer("Users"); // Cosmos collection name
            modelBuilder.Entity<CosmoTest.Models.User>().HasPartitionKey(u => u.Id);
        }
    }
}
