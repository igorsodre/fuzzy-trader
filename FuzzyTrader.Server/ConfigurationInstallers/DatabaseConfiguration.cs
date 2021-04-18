using FuzzyTrader.Server.Data;
using FuzzyTrader.Server.Data.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.Server.ConfigurationInstallers
{
    public class DatabaseConfiguration : IConfigurationInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString("DatabaseUrl"));
            });
            services.AddDefaultIdentity<AppUser>()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}