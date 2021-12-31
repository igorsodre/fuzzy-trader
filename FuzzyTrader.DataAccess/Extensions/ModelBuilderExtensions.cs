using FuzzyTrader.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.DataAccess.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var seeders = typeof(DataContext).Assembly.ExportedTypes
            .Where((x) => typeof(IDatabaseSeeder).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IDatabaseSeeder>()
            .ToList();

        foreach (var seeder in seeders)
        {
            seeder.SeedData(modelBuilder);
        }
    }
}
