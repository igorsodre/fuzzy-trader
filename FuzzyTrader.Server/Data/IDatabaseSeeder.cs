using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Data;

public interface IDatabaseSeeder
{
    public void SeedData(ModelBuilder modelBuilder);
}