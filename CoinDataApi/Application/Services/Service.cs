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
            .Concat(coinbaseData)
            .ToList();

        var totalValue = combinedData.Sum(data => data.PriceClose * data.VolumeTraded);
        var totalVolume = combinedData.Sum(data => data.VolumeTraded);
    
        var vwap = totalVolume != 0 ? totalValue / totalVolume : 0;
        
        return string.Empty;
    }
}

public class AggregatedOhlcvData
{
    public DateTime PeriodStart { get; set; }
    public decimal PriceCloseWeighted { get; set; }
    public decimal TotalVolume { get; set; }
}