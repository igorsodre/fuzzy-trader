using FluentValidation.AspNetCore;
using FuzzyTrader.Server.Filters;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.ConfigurationInstallers;

public class MvcConfiguration : IConfigurationInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var serverSettings = configuration.GetSection("ServerSettings").Get<ServerSettings>();

        services.AddSingleton(serverSettings);
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITradingService, TradingService>();
        services.AddAutoMapper(typeof(Startup));

        services.AddCors(
            options => {
                options.AddDefaultPolicy(
                    policyBuilder => {
                        policyBuilder.WithOrigins(serverSettings.ClientUrls)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    }
                );
            }
        );

        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        services.AddControllers(options => { options.Filters.Add<IncomingRequestDataValidationMiddleware>(); })
            .AddFluentValidation(
                options => {
                    options.DisableDataAnnotationsValidation = true;
                    options.ImplicitlyValidateChildProperties = true;
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                }
            );
    }
}
