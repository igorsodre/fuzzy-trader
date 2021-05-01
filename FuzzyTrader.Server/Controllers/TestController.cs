using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.Contracts.Requests.Investment;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Investment;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly ITradingService _tradingService;
        private readonly IMapper _mapper;

        public TestController(UserManager<AppUser> userManager, DataContext dataContext, ITradingService tradingService,
            IMapper mapper)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _tradingService = tradingService;
            _mapper = mapper;
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

        [HttpPost("list_investment_options")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<GetInvestmentOptionsResponse>>), 200)]
        public async Task<ActionResult> ListInvestmentOption(GetInvestmentOptionsRequest request)
        {
            var result = await _tradingService.GetBestTradingOptionsForCrypto(request.Budget);
            var mappedResutl = _mapper.Map<List<GetInvestmentOptionsResponse>>(result);
            return Ok(new SuccessResponse<List<GetInvestmentOptionsResponse>>(mappedResutl));
        }
    }
}
