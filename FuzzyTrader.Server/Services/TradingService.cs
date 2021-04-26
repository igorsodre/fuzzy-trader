using System.Collections.Generic;
using System.Threading.Tasks;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Services.Iterfaces;

namespace FuzzyTrader.Server.Services
{
    public class TradingService : ITradingService
    {
        public Task<IEnumerable<InvestmentOptions>> GetBestTradingOptions(decimal value)
        {
            throw new System.NotImplementedException();
        }
    }
}
