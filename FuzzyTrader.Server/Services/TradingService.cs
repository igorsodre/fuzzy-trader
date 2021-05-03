using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.Server.Data;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Domain.Investment;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Services
{
    public class TradingService : ITradingService
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public TradingService(DataContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserInvestmentsResult> GetUserInvestments(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new UserInvestmentsResult
                {
                    Errors = new[] {"Invalid user."}
                };
            }

            var investments = await _context.Investments.Include(i => i.Wallet)
                .Where(i => i.Wallet.UserId == userId)
                .Select(i => _mapper.Map<DomainInvestment>(i))
                .ToListAsync();

            return new UserInvestmentsResult
            {
                Success = true,
                Investments = investments
            };
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

        public async Task<InvestmentOrderResult> PlaceInvestmentOrder(InvestmentOrder order)
        {
            var user = await _userManager.FindByIdAsync(order.UserId);
            if (user is null)
            {
                return new InvestmentOrderResult
                {
                    Errors = new[] {"Invalid user."}
                };
            }

            var wallet = await _context.Wallets.SingleOrDefaultAsync(w => w.UserId == user.Id);
            if (wallet is null)
            {
                wallet = new Wallet
                {
                    User = user
                };
                _context.Add(wallet);
                await _context.SaveChangesAsync();
            }

            if (order.IsCrypto)
            {
                return await PlaceCryptoInvestment(order, wallet);
            }

            return await PlaceTradeInvestment(order, wallet);
        }

        private async Task<InvestmentOrderResult> PlaceTradeInvestment(InvestmentOrder order, Wallet wallet)
        {
            var resource = await _context.TradeAssets.FindAsync(order.ProductId);
            if (resource?.Open is null)
            {
                return new InvestmentOrderResult
                {
                    Errors = new[] {"Unavailable investment option."}
                };
            }

            var investment = new Investment
            {
                Description = resource.Symbol,
                Quantity = order.Quantity,
                AssetId = resource.Id,
                Value = order.Quantity * resource.Open.Value,
                Wallet = wallet
            };
            _context.Add(investment);
            await _context.SaveChangesAsync();

            return new InvestmentOrderResult
            {
                Success = true
            };
        }

        private async Task<InvestmentOrderResult> PlaceCryptoInvestment(InvestmentOrder order, Wallet wallet)
        {
            var resource = await _context.CryptoCoins.FindAsync(order.ProductId);
            if (resource is null || !decimal.TryParse(resource.PriceUsd, out var price))
            {
                return new InvestmentOrderResult
                {
                    Errors = new[] {"Unavailable investment option."}
                };
            }

            var investment = new Investment
            {
                Description = resource.AssetId,
                Quantity = order.Quantity,
                AssetId = resource.Id,
                Value = order.Quantity * price,
                Wallet = wallet
            };
            _context.Add(investment);
            await _context.SaveChangesAsync();

            return new InvestmentOrderResult
            {
                Success = true
            };
        }
    }
}
