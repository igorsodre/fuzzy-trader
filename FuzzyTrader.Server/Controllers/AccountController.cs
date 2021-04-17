using System.Threading.Tasks;
using FuzzyTrader.Contracts.Requests.Account;
using FuzzyTrader.Contracts.Responses;
using FuzzyTrader.Contracts.Responses.Account;
using FuzzyTrader.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginResponse>>> Login(LoginRequest request)
        {
            var authResponse = await _accountService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Succsess)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new Response<LoginResponse>(new LoginResponse
            {
                Token = authResponse.Token
            }));
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Signup(SignupRequest request)
        {
            var authResponse = await _accountService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Succsess)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new Response<SignupResponse>(new SignupResponse
            {
                Token = authResponse.Token
            }));
        }
        
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh(SignupRequest request)
        {
            var authResponse = await _accountService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Succsess)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = authResponse.ErrorMessages
                });
            }

            _accountService.AddRefreshTokenForUserOnResponse(authResponse.User, HttpContext.Response);

            return Ok(new Response<SignupResponse>(new SignupResponse
            {
                Token = authResponse.Token
            }));
        }
        
    }
}