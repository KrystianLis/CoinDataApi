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

        var combinedData = bitstampData
            .Concat(coinbaseData);

        var totals = combinedData.Aggregate(
            new { TotalValue = 0m, TotalVolume = 0m }, 
            (acc, data) => new 
            {
                TotalValue = acc.TotalValue + data.PriceClose * data.VolumeTraded,
                TotalVolume = acc.TotalVolume + data.VolumeTraded
            });
    
        var vwap = totals.TotalVolume != 0 ? totals.TotalValue / totals.TotalVolume : 0;
        
        return string.Empty;
    }
}

public class AggregatedOhlcvData
{
    public DateTime PeriodStart { get; set; }
    public decimal PriceCloseWeighted { get; set; }
    public decimal TotalVolume { get; set; }
}