using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CoinAPI.REST.V1;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.MappingProfiles;

namespace FuzzyTrader.Server.Scripts
{
    public class TradeDataGenerator
    {
        private readonly IMapper _mapper;
        private const string MarketStackApiUrl = "http://api.marketstack.com/v1";
        private readonly string _currentDir = Directory.GetCurrentDirectory();
        private readonly HttpClient _httpClient;

        private const string Symbols =
            "MSFT,AAPL,AMZN,GOOG,GOOGL,BABA,FB,BRK.B,BRK.A,VOD,V,JPM,JNJ,WMT,MA,PGTSM,CHT,RHHBF,RHHVF";
        // ",T,UNH,BAC,HD,INTC,KO,VZ,RHHBY,XOM,DIS,MRK,NVS,CMCSA,PFE,PEP,TM,CVX,ADBE,CSCO,WFC,NVDA,NFLX,ORCL,BA,CHL,CRM,SAP,MCD,NKE,ABT,C,MDT,HSBC,TSLA,COST,BMY,HSBC/PA,PYPL,PM,NEE,ABBV,LLY,AMGN,SNY,TMO,ASML,AZN,ACN,IBM,HON,UTX,TOT,AVGO,TXN,NVO,UNP,RY,BP,LMT,AMT,LIN,GSK,DHR,CHTR,HDB,GE,SBUX,BUD,GILD,TD,SHI,QCOM,AXP,RDS.A,FIS,BTI,MMM,LOW,DEO";

        public TradeDataGenerator()
        {
            var mapConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ExternalToDatabaseProfile>(); });
            _mapper = new Mapper(mapConfig);
            _httpClient = new HttpClient();
        }

        public async Task GetCryptoData(string apiKey)
        {
            var coinApiClient = new CoinApiRestClient(apiKey);
            var result = await coinApiClient.Metadata_list_assetsAsync();
            var mappedResult = _mapper.Map<List<CryptoCoin>>(result);

            var filePath = Path.Join(_currentDir, "Data", "Static", "CryptoCoins.json");
            await using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, mappedResult);
        }

        public async Task GetTradingData(string apiKey)
        {
            var enpoint = MarketStackApiUrl + $"/eod?access_key={apiKey}&symbols={Symbols}";
            var response = await _httpClient.GetAsync(enpoint);
            if (!response.IsSuccessStatusCode) return;
            var resutlstring = await response.Content.ReadAsStringAsync();

            var filePath = Path.Join(_currentDir, "Data", "Static", "Trades.json");
            await using var stream = File.Create(filePath);
            await stream.WriteAsync(Encoding.UTF8.GetBytes(resutlstring));
        }
    }
}
