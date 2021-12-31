using System.Text.Json;
using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.DataAccess.Seeders;

public class CryptoCoinSeeder : IDatabaseSeeder
{
    public void SeedData(ModelBuilder modelBuilder)
    {
        var path = Path.Join("Data", "Static", "CryptoCoins.json");
        var jsonString = File.ReadAllText(path);
        var assets = JsonSerializer.Deserialize<List<CryptoCoin>>(jsonString);
        assets = assets?.Where(c => c.TypeIsCrypto).ToList();

        if (assets is null)
            return;

        modelBuilder.Entity<CryptoCoin>().HasData(assets);
    }
}
