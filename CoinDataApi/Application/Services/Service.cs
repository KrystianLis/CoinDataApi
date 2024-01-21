using CoinDataApi.Application.Helper;
using CoinDataApi.Core.Interfaces.Clients;

namespace CoinDataApi.Application.Services;

public class Service : IService
{
    private readonly ICoinApiClient _client;

    public Service(ICoinApiClient client)
    {
        _client = client;
    }

    public async Task<string> AggregateDataAsync(CancellationToken token = default)
    {
        var bitstampData = await _client.GetOhlcvFromLastDay("bitstamp_spot_btc_usd", token: token);
        var coinbaseData = await _client.GetOhlcvFromLastDay("coinbase_spot_btc_usd", token: token);

        var combinedData = bitstampData.Concat(coinbaseData)
            .GroupBy(data => data.TimePeriodStart)
            .Select(group => new AggregatedOhlcvData
            {
                PeriodStart = group.Key,
                PriceCloseWeighted = group.Sum(data => data.PriceClose * data.VolumeTraded) / group.Sum(data => data.VolumeTraded),
                TotalVolume = group.Sum(data => data.VolumeTraded)
            }).ToList();

        var vwap = VwapCalculator.CalculateVwap(combinedData);
        
        return string.Empty;
    }
}

public class AggregatedOhlcvData
{
    public DateTime PeriodStart { get; set; }
    public decimal PriceCloseWeighted { get; set; }
    public decimal TotalVolume { get; set; }
}