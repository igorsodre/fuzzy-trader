using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyTrader.DataAccess.Entities;

public class Investment
{
    [Key]
    public string Id { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    [MaxLength(100)]
    public string AssetId { get; set; }

    public int Quantity { get; set; }

    [MaxLength(450)]
    public string WalletId { get; set; }

    [ForeignKey(nameof(WalletId))]
    public virtual Wallet Wallet { get; set; }
}
