using FuzzyTrader.DataAccess.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.DataAccess.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void MigrateAndSeed(this IApplicationBuilder application, params Type[] containerTypes)
    {
        using var scope = application.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IDataContext>();

        context.Database.Migrate();

        var seeders = containerTypes.SelectMany(t => t.Assembly.ExportedTypes)
            .Where((x) => typeof(IDatabaseSeeder).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Distinct()
            .Select(Activator.CreateInstance)
            .Cast<IDatabaseSeeder>()
            .ToList();

        if (!seeders.Any())
        {
            return;
        }

        foreach (var seeder in seeders)
        {
            seeder.SeedData(context);
        }
    }
}
