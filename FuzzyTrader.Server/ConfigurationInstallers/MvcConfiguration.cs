using FluentValidation.AspNetCore;
using FuzzyTrader.Server.Filters;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.ConfigurationInstallers
{
    public class MvcConfiguration : IConfigurationInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var serverSettings = configuration.GetSection("ServerSettings")
                .Get<ServerSettings>();


            services.AddSingleton(serverSettings);
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options => {
                options.AddDefaultPolicy(policyBuilder => {
                    policyBuilder.WithOrigins(serverSettings.ClientUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddControllers(options => { options.Filters.Add<ValidationFilter>(); })
                .AddFluentValidation(options => {
                    options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    options.ImplicitlyValidateChildProperties = true;
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                });
        }
    }
}
