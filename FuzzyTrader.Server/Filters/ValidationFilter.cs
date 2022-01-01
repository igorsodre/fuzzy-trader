using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FuzzyTrader.Server.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage));

            var errorsList = errors.SelectMany(
                x => x.Value.Select(
                    xx => new ErrorModel
                    {
                        FieldName = x.Key,
                        Message = xx
                    }
                )
            );

            var errorResponse = new ErrorResponse { Errors = errorsList };

            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}
