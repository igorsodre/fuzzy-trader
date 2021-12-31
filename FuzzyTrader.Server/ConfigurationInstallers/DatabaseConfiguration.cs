using System;
using FuzzyTrader.DataAccess.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.ConfigurationInstallers;

public class DatabaseConfiguration : IConfigurationInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccess(configuration.GetConnectionString("DatabaseUrl"));

        services.Configure<DataProtectionTokenProviderOptions>(
            options => { options.TokenLifespan = TimeSpan.FromMinutes(15); }
        );
    }
}
