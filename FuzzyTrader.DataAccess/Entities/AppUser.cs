using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FuzzyTrader.DataAccess.Entities;

public class AppUser : IdentityUser
{
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }

    public int TokenVersion { get; set; }

    public virtual Wallet Wallet { get; set; }
}
