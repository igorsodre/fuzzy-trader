using System.Collections.Generic;
using System.Threading.Tasks;
using FuzzyTrader.Server.Domain;

namespace FuzzyTrader.Server.Services.Iterfaces
{
    public interface ITradingService
    {
        public Task<IEnumerable<InvestmentOptions>> GetBestTradingOptionsForAssets(decimal limit);
        public Task<IEnumerable<InvestmentOptions>> GetBestTradingOptionsForCrypto(decimal limit);
    }
}
