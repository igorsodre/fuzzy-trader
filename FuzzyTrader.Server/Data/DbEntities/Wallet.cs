using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyTrader.Server.Data.DbEntities
{
    public class Wallet
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }

        public virtual List<Investment> Investments { get; set; }
    }
}
