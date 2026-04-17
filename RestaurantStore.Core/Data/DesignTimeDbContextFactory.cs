using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RestaurantStore.Core.Data
{
    public class DesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-QTOHTRT;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}