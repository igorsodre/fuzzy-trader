using System;
using System.Linq;
using FuzzyTrader.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Extensions.Database;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var seeders = typeof(Startup).Assembly.ExportedTypes.Where((x) =>
                typeof(IDatabaseSeeder).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IDatabaseSeeder>()
            .ToList();

        foreach (var seeder in seeders)
        {
            seeder.SeedData(modelBuilder);
        }
    }
}