using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RealStateApp.Infrastructure.Persistence.Contexts;

namespace RealStateApp.Infrastructure.Persistence
{
    public class RealStateAppContextFactory : IDesignTimeDbContextFactory<RealStateAppContext>
    {
        public RealStateAppContext CreateDbContext(string[] args)
        {
            // Buscar el appsettings.json del proyecto WebApp (1 nivel arriba)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../RealStateApp");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<RealStateAppContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new RealStateAppContext(optionsBuilder.Options);
        }
    }
}
