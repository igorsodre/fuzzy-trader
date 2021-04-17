using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Server.Domain;

namespace FuzzyTrader.Server.Services
{
    public interface IAccountService
    {
        public Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}