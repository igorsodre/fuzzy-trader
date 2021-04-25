using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CoinAPI.REST.V1;
using FuzzyTrader.Contracts.External;
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

        private const string Symbols1 =
            "MSFT,AAPL,AMZN,GOOG,GOOGL,BABA,FB,BRK.B,BRK.A,VOD,V,JPM,JNJ,WMT,MA,PGTSM,CHT,RHHBF,RHHVF";

        private const string Symbols2 =
            "T,UNH,BAC,HD,INTC,KO,VZ,RHHBY,XOM,DIS,MRK,NVS,CMCSA,PFE,PEP,TM,CVX,ADBE,CSCO,WFC,NVDA";

        private const string Symbols3 =
            "NFLX,ORCL,BA,CHL,CRM,SAP,MCD,NKE,ABT,C,MDT,HSBC,TSLA,COST,BMY,HSBC/PA,PYPL,PM,NEE,ABBV,LLY,AMGN,SNY,TMO";

        private const string Symbols4 =
            "ASML,AZN,ACN,IBM,HON,UTX,TOT,AVGO,TXN,NVO,UNP,RY,BP,LMT,AMT,LIN,GSK,DHR,CHTR,HDB,GE,SBUX,BUD,GILD,TD,SHI,QCOM,AXP,RDS.A,FIS,BTI,MMM,LOW,DEO";

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
            var enpoint = MarketStackApiUrl + $"/eod/latest?access_key={apiKey}&symbols=";

            var resutlstring1 = await GetResultFromSymbols(enpoint + Symbols1);
            var resutlstring2 = await GetResultFromSymbols(enpoint + Symbols2);
            var resutlstring3 = await GetResultFromSymbols(enpoint + Symbols3);
            var resutlstring4 = await GetResultFromSymbols(enpoint + Symbols4);

            var marketData = CombineMarketResponses(new[] {resutlstring1, resutlstring2, resutlstring3, resutlstring4});
            var filePath = Path.Join(_currentDir, "Data", "Static", "Trades.json");
            await using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, marketData);
        }

        private async Task<string> GetResultFromSymbols(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return string.Empty;
            return await response.Content.ReadAsStringAsync();
        }

        private IEnumerable<MarketStackData> CombineMarketResponses(IEnumerable<string> responses)
        {
            var marketData = new List<MarketStackData>();
            foreach (var response in responses)
            {
                if (string.IsNullOrEmpty(response)) continue;
                try
                {
                    var curatedResponse = response.Replace("[],", "");
                    var marketResponse = JsonSerializer.Deserialize<MartketStackEndOfDayResponse>(curatedResponse);
                    if (marketResponse?.data is {Count: > 0})
                    {
                        marketData.AddRange(marketResponse.data);
                    }
                }
                catch { }
            }

            return marketData;
        }
    }
}
