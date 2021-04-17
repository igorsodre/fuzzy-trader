using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Domain;
using Microsoft.AspNetCore.Identity;

namespace FuzzyTrader.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }


        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Email aready taken"}
                };
            }

            var user = new AppUser()
            {
                Email = email,
                UserName = email,
                TokenVersion = 1
            };

            var createdUser = await _userManager.CreateAsync(user, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description)
                };
            }

            var token = _tokenService.CreateAccessToken(user);

            return new AuthenticationResult
            {
                Succsess = true,
                Token = token
            };
        }
    }
}