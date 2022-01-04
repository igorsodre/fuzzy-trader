using System.Reflection;
using System.Text.Json;
using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;

namespace FuzzyTrader.DataAccess.Seeders;

public class TradeAssetSeeder : IDatabaseSeeder
{
    public void SeedData(IDataContext context, IAccountManager accountManager)
    {
        if (context.TradeAssets.Any())
        {
            return;
        }

        var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.Join(currentDir, "Static", "Trades.json");
        var jsonString = File.ReadAllText(path);
        var assets = JsonSerializer.Deserialize<List<TradeAsset>>(jsonString);

        if (assets is null)
        {
            return;
        }

        context.TradeAssets.AddRange(assets);
        context.SaveChanges();
    }
}
