using TestProj.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TestProj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<Classifier> Classifiers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>()
                .HasOne(e => e.Classifier)
                .WithMany()
                .HasForeignKey(c => c.TypeGuid)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for TypeGuid
            modelBuilder.Entity<Entity>()
                .HasIndex(e => e.TypeGuid)
                .HasName("IX_TypeGuid");
        }
    }
}
