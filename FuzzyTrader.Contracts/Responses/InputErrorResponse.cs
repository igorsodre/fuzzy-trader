using System.Collections.Generic;

namespace FuzzyTrader.Contracts.Responses
{
    public class InputErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}