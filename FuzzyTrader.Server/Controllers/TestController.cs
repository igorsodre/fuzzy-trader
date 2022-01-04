using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.DataAccess.Interfaces;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Controllers;

[ApiController]
[Route("api/test")]
[Produces("application/json")]
public class TestController : ControllerBase
{
    private readonly IAccountManager _accountManager;
    private readonly IDataContext _dataContext;
    private readonly ITradingService _tradingService;
    private readonly IMapper _mapper;

    public TestController(
        IAccountManager accountManager,
        IDataContext dataContext,
        ITradingService tradingService,
        IMapper mapper
    )
    {
        _accountManager = accountManager;
        _dataContext = dataContext;
        _tradingService = tradingService;
        _mapper = mapper;
    }

    [HttpPost("remove-all-users")]
    public async Task<ActionResult> RemoveAllUsers()
    {
        await using var trasaction = await _dataContext.Database.BeginTransactionAsync();

        var users = await _dataContext.Users.ToListAsync();

        foreach (var user in users)
        {
            await _accountManager.DeleteAsync(user);
        }

        await trasaction.CommitAsync();

        return Ok(new SuccessResponse<string>("test"));
    }

    // [HttpPost("list-investment-options")]
    // [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
    // public async Task<ActionResult> ListInvestmentOption()
    // {
    //     return Ok(new SuccessResponse<string>("OK"));
    // }
}
