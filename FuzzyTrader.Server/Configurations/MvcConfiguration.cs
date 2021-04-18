using FluentValidation.AspNetCore;
using FuzzyTrader.Server.Filters;
using FuzzyTrader.Server.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FuzzyTrader.Server.Configurations
{
    public class MvcConfiguration : IConfigurationInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddControllers(options => { options.Filters.Add<ValidationFilter>(); })
                .AddFluentValidation(options => { options.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "FuzzyTrader.Server", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        System.Array.Empty<string>()
                    }
                });
            });
        }
    }
}