using System.Linq;
using System.Threading.Tasks;
using FuzzyTrader.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FuzzyTrader.Server.Filters;

public class IncomingRequestDataValidationMiddleware : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                .SelectMany(
                    validationResult => validationResult.Value.Errors.Select(
                        message => new ErrorModel
                        {
                            FieldName = validationResult.Key,
                            Message = message.ErrorMessage
                        }
                    )
                );

            var errorResponse = new ErrorResponse { Errors = errors };

            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}
