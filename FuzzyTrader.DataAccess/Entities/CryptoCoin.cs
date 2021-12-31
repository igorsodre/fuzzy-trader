using System.ComponentModel.DataAnnotations;

namespace FuzzyTrader.DataAccess.Entities;

public class CryptoCoin
{
    [Key]
    public string Id { get; set; }

    [MaxLength(100)]
    public string AssetId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    public bool TypeIsCrypto { get; set; }

    [MaxLength(100)]
    public string DataQuoteStart { get; set; }

    [MaxLength(100)]
    public string DataQuoteEnd { get; set; }

    [MaxLength(100)]
    public string DataOrderbookStart { get; set; }

    [MaxLength(100)]
    public string DataOrderbookEnd { get; set; }

    [MaxLength(100)]
    public string DataTradeStart { get; set; }

    [MaxLength(100)]
    public string DataTradeEnd { get; set; }

    [MaxLength(100)]
    public string DataQuoteCount { get; set; }

    [MaxLength(100)]
    public string DataTradeCount { get; set; }

    [MaxLength(100)]
    public string DataSymbolsCount { get; set; }

    [MaxLength(100)]
    public string Volume1HrsUsd { get; set; }

    [MaxLength(100)]
    public string Volume1DayUsd { get; set; }

    [MaxLength(100)]
    public string Volume1MthUsd { get; set; }

    [MaxLength(100)]
    public string PriceUsd { get; set; }
}
