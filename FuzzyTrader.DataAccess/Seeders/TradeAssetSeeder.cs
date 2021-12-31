using System.Text.Json;
using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.DataAccess.Seeders;

public class TradeAssetSeeder : IDatabaseSeeder
{
    public void SeedData(ModelBuilder modelBuilder)
    {
        var path = Path.Join("Data", "Static", "Trades.json");
        var jsonString = File.ReadAllText(path);
        var assets = JsonSerializer.Deserialize<List<TradeAsset>>(jsonString);

        if (assets is null) return;

        modelBuilder.Entity<TradeAsset>()
            .HasData(assets);
    }
}