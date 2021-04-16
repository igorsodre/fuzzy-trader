using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.Configurations
{
    public interface IConfigurationInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}