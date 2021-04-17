using System.Collections.Generic;

namespace FuzzyTrader.Contracts.Responses
{
    public class ErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}