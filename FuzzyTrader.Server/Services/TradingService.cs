using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Server.Data;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Services
{
    public class TradingService : ITradingService
    {
        private readonly DataContext _context;

        public TradingService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvestmentOptions>> GetBestTradingOptionsForAssets(decimal limit)
        {
            var assets = await _context.TradeAssets.Where(asset => asset.Open > 0 && asset.Open <= limit)
                .Take(50)
                .ToListAsync();

            var result = new List<InvestmentOptions>();

            foreach (var asset in assets)
            {
                if (asset.Open is null or 0) continue;

                var quantity = (int) (limit / asset.Open);
                var investment = new InvestmentOptions
                {
                    Description = asset.Symbol,
                    Quantity = quantity,
                    BaseValue = asset.Open,
                    ProductId = asset.Id,
                    TotalValue = quantity * asset.Open,
                    DailyTradedVolume = asset.Volume.ToString(),
                    IsCrypto = false
                };
                result.Add(investment);
            }

            result.Sort((item0, item1) => item1.TotalValue > item0.TotalValue ? 1 : -1);

            return result;
        }

        public async Task<IEnumerable<InvestmentOptions>> GetBestTradingOptionsForCrypto(decimal limit)
        {
            var coins = await _context.CryptoCoins
                .Where((c) =>
                    c.PriceUsd != null &&
                    Convert.ToDecimal(c.PriceUsd) > (decimal) 0.5 &&
                    Convert.ToDecimal(c.PriceUsd) <= limit)
                .Take(50)
                .ToListAsync();

            var result = new List<InvestmentOptions>();

            foreach (var coin in coins)
            {
                var parseSuccess = decimal.TryParse(coin.PriceUsd, out var price);

                if (!parseSuccess) continue;

                var quantity = (int) (limit / price);
                var investment = new InvestmentOptions
                {
                    Description = coin.Name,
                    Quantity = quantity,
                    BaseValue = price,
                    ProductId = coin.Id,
                    TotalValue = quantity * price,
                    DailyTradedVolume = coin.Volume1DayUsd,
                    IsCrypto = true
                };
                result.Add(investment);
            }

            result.Sort((item0, item1) => item1.TotalValue > item0.TotalValue ? 1 : -1);

            return result;
        }
    }
}
