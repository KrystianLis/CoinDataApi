using CoinDataApi.Core.Models;

namespace CoinDataApi.Application.Tools;

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
    
    public static IEnumerable<OhlcvData> FindPointsWithHighDeviation(IReadOnlyList<OhlcvData> data, decimal vwap, double standardDeviation)
    {
        var result = new List<OhlcvData>();

        foreach (var point in data)
        {
            var deviation = Math.Abs(point.ClosePrice - vwap);
            if (deviation > (decimal)standardDeviation)
            {
                result.Add(point);
            }
        }

        return result;
    }
}