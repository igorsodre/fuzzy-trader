using System.ComponentModel.DataAnnotations;

namespace FuzzyTrader.Server.Data.DbEntities
{
    public class TradeAsset
    {
        [Key]
        public string Id { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public decimal? Volume { get; set; }
        public decimal? AdjHigh { get; set; }
        public decimal? AdjLow { get; set; }
        public decimal? AdjClose { get; set; }
        public decimal? AdjOpen { get; set; }
        public decimal? AdjVolume { get; set; }
        public decimal? SplitFactor { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public string Date { get; set; }
    }
}
