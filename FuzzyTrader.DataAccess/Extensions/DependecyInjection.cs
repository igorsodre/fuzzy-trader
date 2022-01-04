using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FuzzyTrader.DataAccess.Extensions;

public static class DependecyInjection
{
    public static void AddDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<DataContext>(
            options => {
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)
                );
            }
        );
        services.AddDefaultIdentity<AppUser>().AddEntityFrameworkStores<DataContext>();
        services.AddScoped<IDataContext>(provider => provider.GetRequiredService<DataContext>());
        services.AddScoped<IAccountManager, AccountManager>();
    }
}
