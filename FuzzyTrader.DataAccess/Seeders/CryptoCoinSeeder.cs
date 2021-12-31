using System.Reflection;
using System.Text.Json;
using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.DataAccess.Seeders;

public class CryptoCoinSeeder : IDatabaseSeeder
{
    public void SeedData(IDataContext context)
    {
        if (context.CryptoCoins.Any())
        {
            return;
        }

        var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.Join(currentDir, "Static", "CryptoCoins.json");
        var jsonString = File.ReadAllText(path);
        var assets = JsonSerializer.Deserialize<List<CryptoCoin>>(jsonString);
        assets = assets?.Where(c => c.TypeIsCrypto).ToList();

        if (assets is null)
            return;

        context.CryptoCoins.AddRange(assets);
        context.SaveChanges();
    }
}
