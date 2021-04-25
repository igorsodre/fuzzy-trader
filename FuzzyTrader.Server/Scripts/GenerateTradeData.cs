using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CoinAPI.REST.V1;
using FuzzyTrader.Server.Data.DbEntities;
using FuzzyTrader.Server.MappingProfiles;

namespace FuzzyTrader.Server.Scripts
{
    public class GenerateTradeData
    {
        private readonly IMapper _mapper;

        public GenerateTradeData()
        {
            var mapConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ExternalToDatabaseProfile>(); });
            _mapper = new Mapper(mapConfig);
        }

        public async Task Get(string apiKey)
        {
            var coinApiClient = new CoinApiRestClient(apiKey);
            var result = await coinApiClient.Metadata_list_assetsAsync();
            var mappedResult = _mapper.Map<List<CryptoCoin>>(result);

            var currentDir = Directory.GetCurrentDirectory();
            var filePath = Path.Join(currentDir, "..", "Data", "Static", "Trades.json");
            await using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, mappedResult);
        }
    }
}
