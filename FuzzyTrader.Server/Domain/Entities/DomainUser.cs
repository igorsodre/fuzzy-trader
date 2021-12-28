using System;

namespace FuzzyTrader.Server.Domain.Entities;

public class DomainUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UInt32 TokenVersion { get; set; }
    public string UserName { get; set; }
}