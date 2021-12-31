using FuzzyTrader.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FuzzyTrader.DataAccess.Interfaces;

public interface IDataContext
{
    DbSet<Wallet> Wallets { get; set; }

    DbSet<Investment> Investments { get; set; }

    DbSet<CryptoCoin> CryptoCoins { get; set; }

    DbSet<TradeAsset> TradeAssets { get; set; }

    DatabaseFacade Database { get; }

    ChangeTracker ChangeTracker { get; }

    DbContextId ContextId { get; }

    DbSet<AppUser> Users { get; set; }

    DbSet<IdentityUserClaim<string>> UserClaims { get; set; }

    DbSet<IdentityUserLogin<string>> UserLogins { get; set; }

    DbSet<IdentityUserToken<string>> UserTokens { get; set; }

    DbSet<IdentityUserRole<string>> UserRoles { get; set; }

    DbSet<IdentityRole> Roles { get; set; }

    DbSet<IdentityRoleClaim<string>> RoleClaims { get; set; }

    int SaveChanges();

    int SaveChanges(bool acceptAllChangesOnSuccess);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);

    void Dispose();

    ValueTask DisposeAsync();

    EntityEntry Add(object entity);

    ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken);

    EntityEntry Attach(object entity);

    EntityEntry Update(object entity);

    EntityEntry Remove(object entity);

    void AddRange(params object[] entities);

    void AddRange(IEnumerable<object> entities);

    Task AddRangeAsync(params object[] entities);

    Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken);

    void AttachRange(params object[] entities);

    void AttachRange(IEnumerable<object> entities);

    void UpdateRange(params object[] entities);

    void UpdateRange(IEnumerable<object> entities);

    void RemoveRange(params object[] entities);

    void RemoveRange(IEnumerable<object> entities);

    object? Find(Type entityType, params object?[]? keyValues);

    ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues);

    ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken);

    event EventHandler<SavingChangesEventArgs>? SavingChanges;

    event EventHandler<SavedChangesEventArgs>? SavedChanges;

    event EventHandler<SaveChangesFailedEventArgs>? SaveChangesFailed;
}
