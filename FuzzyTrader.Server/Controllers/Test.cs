using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Server.Data;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Extensions;
using FuzzyTrader.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Controllers
{
    [ApiController]
    public class Test : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public Test(DataContext dataContext, ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("/test")]
        public  IActionResult Get()
        {
            return Ok(HttpContext.GetUserId());
        }

        [HttpGet("/test2")]
        public IActionResult Get2(string param1)
        {
            var result = _tokenService.VerifyRefreshToken(param1);

            return Ok(result);
        }
    }
}