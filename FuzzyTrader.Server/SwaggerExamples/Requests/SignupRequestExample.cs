using FuzzyTrader.Contracts.Requests.Account;
using Swashbuckle.AspNetCore.Filters;

namespace FuzzyTrader.Server.SwaggerExamples.Requests
{
    public class SignupRequestExample : IExamplesProvider<SignupRequest>
    {
        public SignupRequest GetExamples()
        {
            return new SignupRequest
            {
                Email = "dotnettest1@mailinator.com",
                Password = "Password!1"
            };
        }
    }
}
