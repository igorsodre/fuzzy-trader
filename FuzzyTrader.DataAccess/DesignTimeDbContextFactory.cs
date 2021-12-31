using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FuzzyTrader.DataAccess;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<DataContext>();
        builder.UseSqlServer(MigrationsConfig.GetConnectionString());
        return new DataContext(builder.Options);
    }
}
