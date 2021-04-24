using System.ComponentModel.DataAnnotations;

namespace FuzzyTrader.Server.Data.DbEntities
{
    public class CryptoCoin
    {
        [Key]
        public string Id { get; set; }

        public string AssetId { get; set; }
        public string Name { get; set; }
        public bool TypeIsCrypto { get; set; }
        public string DataQuoteStart { get; set; }
        public string DataQuoteEnd { get; set; }
        public string DataOrderbookStart { get; set; }
        public string DataOrderbookEnd { get; set; }
        public string DataTradeStart { get; set; }
        public string DataTradeEnd { get; set; }
        public string DataQuoteCount { get; set; }
        public string DataTradeCount { get; set; }
        public string DataSymbolsCount { get; set; }
        public string Volume1HrsUsd { get; set; }
        public string Volume1DayUsd { get; set; }
        public string Volume1MthUsd { get; set; }
        public string PriceUsd { get; set; }
    }
}
