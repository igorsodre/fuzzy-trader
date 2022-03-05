using System.Collections.Generic;

namespace FuzzyTrader.Server.Domain;

public class DefaultResult
{
    public bool Success { get; set; }

    public IList<string> Errors { get; set; } = new List<string>();
}

public class DefaultResult<T> : DefaultResult
{
    public DefaultResult() { }

    public DefaultResult(T data)
    {
        Data = data;
        Success = true;
    }

    public T Data { get; set; }
}
