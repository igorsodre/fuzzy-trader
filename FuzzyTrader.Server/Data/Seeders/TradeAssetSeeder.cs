using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FuzzyTrader.Server.Data.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Data.Seeders;

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