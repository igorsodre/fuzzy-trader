using System.Collections.Generic;
using System.Threading.Tasks;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Domain.Investment;

namespace FuzzyTrader.Server.Services.Iterfaces;

public interface ITradingService
{
    public Task<UserInvestmentsResult> GetUserInvestments(string userId);
    public Task<IEnumerable<InvestmentOptions>> GetBestTradingOptionsForAssets(decimal limit);
    public Task<IEnumerable<InvestmentOptions>> GetBestTradingOptionsForCrypto(decimal limit);
    public Task<InvestmentOrderResult> PlaceInvestmentOrder(InvestmentOrder order);
}