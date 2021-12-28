using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyTrader.Server.Data.DbEntities;

public class Investment
{
    [Key]
    public string Id { get; set; }

    public string Description { get; set; }

    public decimal Value { get; set; }

    public string AssetId { get; set; }

    public int Quantity { get; set; }
    public string WalletId { get; set; }

    [ForeignKey(nameof(WalletId))]
    public Wallet Wallet { get; set; }
}