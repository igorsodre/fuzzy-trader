using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;

namespace FuzzyTrader.DataAccess.Seeders;

public class UserAccountsSeeder : IDatabaseSeeder
{
    public void SeedData(IDataContext context, IAccountManager accountManager)
    {
        if (accountManager.Users.Any())
        {
            return;
        }

        accountManager.CreateAsync(
                new AppUser
                {
                    Email = "dotnettest1@localhost.com",
                    Name = "Test Name",
                    UserName = "dotnettest1@localhost.com",
                    NormalizedEmail = "dotnettest1@localhost.com".ToUpper(),
                    NormalizedUserName = "dotnettest1@localhost.com".ToUpper(),
                    EmailConfirmed = true
                },
                "Password!1"
            )
            .GetAwaiter()
            .GetResult();
    }
}
