using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FuzzyTrader.Server.Data.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Data.Seeders
{
    public class CryptoCoinSeeder : IDatabaseSeeder
    {
        public void SeedData(ModelBuilder modelBuilder)
        {
            var path = Path.Join("Data", "Static", "CryptoCoins.json");
            var jsonString = File.ReadAllText(path);
            var assets = JsonSerializer.Deserialize<List<CryptoCoin>>(jsonString);
            assets = assets?.Where(c => c.TypeIsCrypto)
                .ToList();

            if (assets is null) return;

            foreach (var asset in assets)
            {
                asset.Id = Guid.NewGuid()
                    .ToString();
            }

            modelBuilder.Entity<CryptoCoin>()
                .HasData(assets);
        }
    }
}
