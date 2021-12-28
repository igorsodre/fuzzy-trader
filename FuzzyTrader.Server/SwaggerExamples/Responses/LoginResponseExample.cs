using FuzzyTrader.Contracts.Responses.Account;
using Swashbuckle.AspNetCore.Filters;

namespace FuzzyTrader.Server.SwaggerExamples.Responses;

public class LoginResponseExample : IExamplesProvider<LoginResponse>
{
    public LoginResponse GetExamples()
    {
        return new LoginResponse
        {
            AccessToken = "A_VALID_JWT_TOKEN"
        };
    }
}