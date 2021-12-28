using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.Extensions.Database;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.Server.Data;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Investment> Investments { get; set; }
    public DbSet<CryptoCoin> CryptoCoins { get; set; }
    public DbSet<TradeAsset> TradeAssets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Seed();
    }
}