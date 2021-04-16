using FuzzyTrader.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.Configurations
{
    public class DatabaseConfiguration : IConfigurationInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString("DatabaseUrl"));
            });
        }
    }
}