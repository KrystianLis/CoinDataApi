using CoinDataApi.Application.Services;

namespace CoinDataApi.Application.Helper;

public static class VwapCalculator
{
    public static decimal CalculateVwap(IReadOnlyList<AggregatedOhlcvData> data)
    {
        var totalValue = data.Sum(ohlcvData => ohlcvData.PriceCloseWeighted * ohlcvData.TotalVolume);
        var totalVolume = data.Sum(ohlcvData => ohlcvData.TotalVolume);
        return totalVolume != 0 ? totalValue / totalVolume : 0;
    }
}