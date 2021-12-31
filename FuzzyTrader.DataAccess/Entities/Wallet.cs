using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyTrader.DataAccess.Entities;

public class Wallet
{
    [Key]
    public string Id { get; set; }

    [MaxLength(450)]
    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; }

    public virtual ICollection<Investment> Investments { get; set; } = new List<Investment>();
}
