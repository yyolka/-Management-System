using DatabaseModels.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext
{
    public class UniversityDbContext : DbContext
    {
        public DbSet<GroupDb> Groups { get; set; }
        public DbSet<StudentDb> Students { get; set; }
        public DbSet<CuratorDb> Curators { get; set; }

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GroupDb>()
                .HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupDb>()
                .HasOne(g => g.Curator)
                .WithOne(c => c.Group)
                .HasForeignKey<CuratorDb>(c => c.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CuratorDb>()
                .HasIndex(c => c.GroupId)
                .IsUnique();
                }
    }
}