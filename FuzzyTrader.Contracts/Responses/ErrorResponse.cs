using System.Collections.Generic;
using System.Linq;

namespace FuzzyTrader.Contracts.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(string[] errorMessages)
        {
            Errors = new List<ErrorModel>(errorMessages.Select(e => new ErrorModel {Message = e}));
        }

        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}