using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.Contracts.Requests.Investment;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Investment;
using FuzzyTrader.Server.Domain.Investment;
using FuzzyTrader.Server.Extensions;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers;

[Route("api/investment")]
[Authorize]
public class InvestmentController : BaseController
{
    private readonly ITradingService _tradingService;
    private readonly IMapper _mapper;

    public InvestmentController(ITradingService tradingService, IMapper mapper)
    {
        _tradingService = tradingService;
        _mapper = mapper;
    }

    [HttpGet("get-investments")]
    [ProducesResponseType(typeof(SuccessResponse<IEnumerable<GetUserInvestmentsResponse>>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> GetInvestments()
    {
        var result = await _tradingService.GetUserInvestments(HttpContext.GetUserId());
        if (!result.Success)
        {
            return BadRequest(new ErrorResponse(result.Errors));
        }

        return Ok(new SuccessResponse<IEnumerable<GetUserInvestmentsResponse>>(
            _mapper.Map<IEnumerable<GetUserInvestmentsResponse>>(result.Investments)));
    }

    [HttpGet("get-investment-options")]
    [ProducesResponseType(typeof(SuccessResponse<IEnumerable<GetInvestmentOptionsResponse>>), 200)]
    public async Task<IActionResult> GetInvestmentOptions(decimal budget)
    {
        var cryptoInvestments = await _tradingService.GetBestTradingOptionsForCrypto(budget);
        var tradingInvesments = await _tradingService.GetBestTradingOptionsForAssets(budget);

        var mappedCryptoInvestments = _mapper.Map<List<GetInvestmentOptionsResponse>>(cryptoInvestments);
        var mappedTradingInvestments = _mapper.Map<List<GetInvestmentOptionsResponse>>(tradingInvesments);

        return Ok(new SuccessResponse<IEnumerable<GetInvestmentOptionsResponse>>(
            mappedCryptoInvestments.Concat(mappedTradingInvestments)));
    }

    [HttpPost("place-investment")]
    [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    public async Task<IActionResult> PlaceInvestment(PlaceInvestmentRequest request)
    {
        var order = _mapper.Map<InvestmentOrder>(request);
        order.UserId = HttpContext.GetUserId();
        var result = await _tradingService.PlaceInvestmentOrder(order);
        if (!result.Success)
        {
            return BadRequest(new ErrorResponse(result.Errors));
        }

        return Ok(SuccessResponse.DefaultOkResponse());
    }
}