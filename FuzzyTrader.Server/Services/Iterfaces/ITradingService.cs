using System.Collections.Generic;
using System.Threading.Tasks;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Domain.Entities;
using FuzzyTrader.Server.Domain.Investment;

namespace FuzzyTrader.Server.Services.Iterfaces;

public interface ITradingService
{
    public Task<DefaultResult<IList<DomainInvestment>>> GetUserInvestments(string userId);

    public Task<IList<InvestmentOptions>> GetBestTradingOptionsForAssets(decimal limit);

    public Task<IList<InvestmentOptions>> GetBestTradingOptionsForCrypto(decimal limit);

    public Task<DefaultResult> PlaceInvestmentOrder(InvestmentOrder order);
}
