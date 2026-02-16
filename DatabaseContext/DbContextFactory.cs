using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseContext
{
    public class UniversityDbContextFactory : IDesignTimeDbContextFactory<UniversityDbContext>
    {
        public UniversityDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Host=localhost;Database=UniversityDb;Username=postgres;Password=336396";
            
            var optionsBuilder = new DbContextOptionsBuilder<UniversityDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            
            return new UniversityDbContext(optionsBuilder.Options);
        }
    }
}