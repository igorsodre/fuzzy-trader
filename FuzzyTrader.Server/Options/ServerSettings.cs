namespace FuzzyTrader.Server.Options;

public class ServerSettings
{
    public string BaseUrl { get; init; }

    public string[] ClientUrls { get; set; }

    public string ResetPasswordRoute { get; set; }
}
