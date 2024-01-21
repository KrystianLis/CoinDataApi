using CoinDataApi.Application.Services;
using CoinDataApi.Core.Models;

namespace CoinDataApi.Application.Helper;

public static class VwapCalculator
{
    public static decimal CalculateVwap(IReadOnlyList<AggregatedOhlcvData> data)
    {
        var totalValue = data.Sum(ohlcvData => ohlcvData.PriceCloseWeighted * ohlcvData.TotalVolume);
        var totalVolume = data.Sum(ohlcvData => ohlcvData.TotalVolume);
        return totalVolume != 0 ? totalValue / totalVolume : 0;
    }

    public static double CalculateStandardDeviation(IReadOnlyList<AggregatedOhlcvData> data, decimal vwap)
    {
        var squaredDifferencesSum = data.Sum(ohlcvData => Math.Pow((double)(ohlcvData.PriceCloseWeighted - vwap), 2));
        var count = data.Count;

        return count > 0 ? Math.Sqrt(squaredDifferencesSum / count) : 0;
    }
}