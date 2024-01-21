using CoinDataApi.Core.Interfaces.Clients;
using CoinDataApi.Core.Models;
using CoinDataApi.Tools;

namespace CoinDataApi.Application.Services;

public class DataAggregatorService : IDataAggregatorService
{
    private readonly ICoinApiClient _client;

    public DataAggregatorService(ICoinApiClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<OhlcvData>> AggregateDataAsync(CancellationToken token = default)
    {
        var bitstampData = await _client.GetOhlcvFromLastDay("bitstamp_spot_btc_usd", token: token);
        var coinbaseData = await _client.GetOhlcvFromLastDay("coinbase_spot_btc_usd", token: token);

        var combinedData = bitstampData.Concat(coinbaseData)
            .GroupBy(data => data.TimePeriodStart)
            .Select(group => new OhlcvData
            {
                TimePeriodStart = group.Key,
                ClosePrice = group.Sum(data => data.ClosePrice * data.TotalVolume) / group.Sum(data => data.TotalVolume),
                TotalVolume = group.Sum(data => data.TotalVolume)
            }).ToList();

        var vwap = VwapCalculator.CalculateVwap(combinedData);
        var standardDeviation = VwapCalculator.CalculateStandardDeviation(combinedData, vwap);

        var resultPoints = VwapCalculator.FindPointsWithHighDeviation(combinedData, vwap, standardDeviation);
        
        return resultPoints;
    }
}