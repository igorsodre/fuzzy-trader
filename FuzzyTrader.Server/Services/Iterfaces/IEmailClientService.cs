using System.Threading.Tasks;
using FuzzyTrader.Server.Domain;

namespace FuzzyTrader.Server.Services.Iterfaces;

public interface IEmailClientService
{
    public Task<DefaultResult> SendEmailAsync(EmailMessage emailMessage);
}