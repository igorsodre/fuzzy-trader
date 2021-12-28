namespace FuzzyTrader.Server.Options;

public class ServerSettings
{
    public string BaseUrl { get; init; }
    public string ClientUrl { get; set; }
    public string ResetPasswordRoute { get; set; }
}