using FuzzyTrader.Server.Data.DbEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Investment> Investments { get; set; }
    }
}
