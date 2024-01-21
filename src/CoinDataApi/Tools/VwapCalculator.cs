using CoinDataApi.Core.Models;

namespace CoinDataApi.Tools;

public static class VwapCalculator
{
    public static decimal CalculateVwap(IReadOnlyList<OhlcvData> data)
    {
        var totalValue = data.Sum(ohlcvData => ohlcvData.ClosePrice * ohlcvData.TotalVolume);
        var totalVolume = data.Sum(ohlcvData => ohlcvData.TotalVolume);
        return totalVolume != 0 ? totalValue / totalVolume : 0;
    }

    public static double CalculateStandardDeviation(IReadOnlyList<OhlcvData> data, decimal vwap)
    {
        var squaredDifferencesSum = data.Sum(ohlcvData => Math.Pow((double)(ohlcvData.ClosePrice - vwap), 2));
        var count = data.Count;

        return count > 0 ? Math.Sqrt(squaredDifferencesSum / count) : 0;
    }
}