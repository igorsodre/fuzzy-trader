using FuzzyTrader.DataAccess.Entities;
using FuzzyTrader.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuzzyTrader.DataAccess;

public class DataContext : IdentityDbContext<AppUser>, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Wallet> Wallets { get; set; }

    public DbSet<Investment> Investments { get; set; }

    public DbSet<CryptoCoin> CryptoCoins { get; set; }

    public DbSet<TradeAsset> TradeAssets { get; set; }
}
