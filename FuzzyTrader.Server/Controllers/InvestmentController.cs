using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FuzzyTrader.Contracts.Requests.Investment;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Investment;
using FuzzyTrader.Server.Domain;
using FuzzyTrader.Server.Extensions;
using FuzzyTrader.Server.Services.Iterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    [Route("api/investment")]
    [Produces("application/json")]
    [Authorize]
    public class InvestmentController : ControllerBase
    {
        private readonly ITradingService _tradingService;
        private readonly IMapper _mapper;

        public InvestmentController(ITradingService tradingService, IMapper mapper)
        {
            _tradingService = tradingService;
            _mapper = mapper;
        }

        [HttpGet("get_investment_options")]
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

        [HttpPost("place_investment")]
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
}