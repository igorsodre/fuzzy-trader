using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyTrader.DataAccess.Entities;

public class TradeAsset
{
    [Key]
    public string Id { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Open { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? High { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Low { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Close { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Volume { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AdjHigh { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AdjLow { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AdjClose { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AdjOpen { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AdjVolume { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? SplitFactor { get; set; }

    [MaxLength(100)]
    public string Symbol { get; set; }

    [MaxLength(100)]
    public string Exchange { get; set; }

    [MaxLength(100)]
    public string Date { get; set; }
}
