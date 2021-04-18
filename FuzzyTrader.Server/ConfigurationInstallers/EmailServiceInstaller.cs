using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.ConfigurationInstallers
{
    public class EmailServiceInstaller : IConfigurationInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var notificationMetadata =
                configuration.GetSection("NotificationMetadata").Get<NotificationMetadata>();
            services.AddSingleton(notificationMetadata);

            services.AddScoped<IEmailClientService, EmailClientService>();
        }
    }
}