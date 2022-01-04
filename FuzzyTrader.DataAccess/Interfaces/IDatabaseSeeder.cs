using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.DataAccess.Interfaces;

public interface IDatabaseSeeder
{
    public void SeedData(IDataContext context, IAccountManager accountManager);
}