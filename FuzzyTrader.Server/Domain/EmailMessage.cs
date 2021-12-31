namespace FuzzyTrader.Server.Domain;

public class EmailMessage
{
    public string Reciever { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}