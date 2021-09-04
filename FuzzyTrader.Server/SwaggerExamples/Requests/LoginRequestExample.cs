using FuzzyTrader.Contracts.Requests.Account;
using Swashbuckle.AspNetCore.Filters;

namespace FuzzyTrader.Server.SwaggerExamples.Requests
{
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Email = "dotnettest1@localhost.com",
                Password = "Password!1"
            };
        }
    }
}
