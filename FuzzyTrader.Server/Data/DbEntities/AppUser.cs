using System;
using Microsoft.AspNetCore.Identity;

namespace FuzzyTrader.Server.Data.DbEntities
{
    public class AppUser : IdentityUser
    {
        public UInt32 TokenVersion { get; set; }
    }
}