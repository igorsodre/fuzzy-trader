using System.Collections.Generic;

namespace FuzzyTrader.Server.Domain;

public class DefaultResult
{
    public bool Success { get; set; }

    public IEnumerable<string> ErrorMessages { get; set; }
}