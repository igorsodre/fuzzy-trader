using System.Threading.Tasks;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Server.Data;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Options;
using FuzzyTrader.Server.Scripts;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    [Route("api/test")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly ServerSettings _serverSettings;

        public TestController(IAccountService accountService,
            UserManager<AppUser> userManager, DataContext dataContext, ServerSettings serverSettings)
        {
            _accountService = accountService;
            _userManager = userManager;
            _dataContext = dataContext;
            _serverSettings = serverSettings;
        }

        [HttpPost("remove_all_users")]
        public async Task<ActionResult> RemoveAllUsers()
        {
            await using var trasaction = await _dataContext.Database.BeginTransactionAsync();

            var users = await _dataContext.Users.ToListAsync();

            foreach (var user in users)
            {
                await _userManager.DeleteAsync(user);
            }

            await trasaction.CommitAsync();

            return Ok(new SuccessResponse<string>("test"));
        }

        [HttpGet("list_investment_options")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        public async Task<ActionResult> ListInvestmentOption()
        {
            return Ok(new SuccessResponse<string>("OK"));
        }
    }
}
