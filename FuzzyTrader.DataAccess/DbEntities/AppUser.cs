using System;
using Microsoft.AspNetCore.Identity;

namespace FuzzyTrader.Server.Data.DbEntities;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
    public UInt32 TokenVersion { get; set; }

    public virtual Wallet Wallet { get; set; }
}