using System.Collections.Generic;

namespace FuzzyTrader.Contracts.Responses
{
    public class BusinessErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}