using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace FuzzyTrader.DataAccess;

public static class MigrationsConfig
{
    public static string GetConnectionString()
    {
        var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var filePath = Path.Combine(buildDir, "databasesettings.json");
        return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(filePath)
            .AddEnvironmentVariables()
            .Build()
            .GetConnectionString("SqlServer");
    }
}
