using System;
using System.Linq;
using FuzzyTrader.Server.ConfigurationInstallers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.Extensions.Configuration;

public static class InstallerExtensions
{
    public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationInstallers = typeof(Startup).Assembly.ExportedTypes
            .Where((x) => typeof(IConfigurationInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IConfigurationInstaller>()
            .ToList();

        foreach (var installer in configurationInstallers)
        {
            installer.InstallServices(services, configuration);
        }
    }
}
