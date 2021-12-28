using FuzzyTrader.Contracts.Requests.Account;
using Swashbuckle.AspNetCore.Filters;

namespace FuzzyTrader.Server.SwaggerExamples.Requests;

public class SignupRequestExample : IExamplesProvider<SignupRequest>
{
    public SignupRequest GetExamples()
    {
        return new SignupRequest
        {
            Email = "dotnettest1@localhost.com",
            Password = "Password!1",
            Name = "Some Name",
            ConfirmedPassword = "Password!1"
        };
    }
}