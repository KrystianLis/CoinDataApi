using CoinDataApi.Application.DTOs;
using CoinDataApi.Application.Tools;
using CoinDataApi.Core.Interfaces.Clients;
using CoinDataApi.Core.Models;

namespace CoinDataApi.Application.Services;

public class DataAggregatorService : IDataAggregatorService
{
    private readonly ICoinApiClient _client;

    public DataAggregatorService(ICoinApiClient client)
    {
        _client = client;
    }

    public async Task<PriceAnalysisResultDto> AggregateDataAsync(string timeStart, string timeEnd, string periodId, CancellationToken token = default)
    {
        // Due to the CoinApi restrictions, it is not possible to use Task.WhenAll()
        var bitstampData24H = await _client.Get24hLatestData("bitstamp_spot_btc_usd", token: token);
        var coinbaseData24H = await _client.Get24hLatestData("coinbase_spot_btc_usd", token: token);
        var bitstampData = await _client.GetHistoricalData("bitstamp_spot_btc_usd", timeStart, timeEnd, periodId, token: token);
        var coinbaseData = await _client.GetHistoricalData("coinbase_spot_btc_usd", timeStart, timeEnd, periodId, token: token);
        
        var latest24HDataTask = Task.Run(() => AnalyzeMarketVolatility(bitstampData24H, coinbaseData24H), token);
        var historicalDataTask = Task.Run(() => AnalyzeMarketVolatility(bitstampData, coinbaseData), token);

        await Task.WhenAll(latest24HDataTask, historicalDataTask);

        var (vwap24H, standardDeviation24H, pointsWithHighDeviation24H) = latest24HDataTask.Result;
        var (vwap, standardDeviation, pointsWithHighDeviation) = historicalDataTask.Result;
        
        return new PriceAnalysisResultDto
        {
            Vwap24H = vwap24H,
            StandardDeviation24H = standardDeviation24H,
            PointsWithHighDeviation24H = pointsWithHighDeviation24H.Select(x => new OhlcvDataDto
            {
                ClosePrice = x.ClosePrice,
                TimePeriodStart = x.TimePeriodStart,
                TimePeriodEnd = x.TimePeriodEnd,
                TotalVolume = x.TotalVolume
            }),

            Vwap = vwap,
            StandardDeviation = standardDeviation,
            PointsWithHighDeviation = pointsWithHighDeviation.Select(x => new OhlcvDataDto
            {
                ClosePrice = x.ClosePrice,
                TimePeriodStart = x.TimePeriodStart,
                TimePeriodEnd = x.TimePeriodEnd,
                TotalVolume = x.TotalVolume
            })
        };
    }

    private static (decimal vwap24H, double standardDeviation24H, IEnumerable<OhlcvData> pointsWithHighDeviation24H)
        AnalyzeMarketVolatility(IReadOnlyCollection<OhlcvData> bitstampData, IReadOnlyCollection<OhlcvData> coinbaseData)
    {
        var combinedData = CombinedData(bitstampData, coinbaseData);
        
        var vwap = VwapCalculator.CalculateVwap(combinedData);
        var standardDeviation = VwapCalculator.CalculateStandardDeviation(combinedData, vwap);
        
        var pointsWithHighDeviation = VwapCalculator.FindPointsWithHighDeviation(combinedData, vwap, standardDeviation);
        return (vwap, standardDeviation, pointsWithHighDeviation);
    }

    private static List<OhlcvData> CombinedData(IReadOnlyCollection<OhlcvData> bitstampData, IReadOnlyCollection<OhlcvData> coinbaseData)
    {
        // Combine data from two sources and calculate the weighted average
        var combinedData = bitstampData.Concat(coinbaseData)
            .GroupBy(data => data.TimePeriodStart)
            .Select(group => new OhlcvData
            {
                TimePeriodStart = group.Key,
                TimePeriodEnd = group.First().TimePeriodEnd,
                ClosePrice = group.Sum(data => data.ClosePrice * data.TotalVolume) / group.Sum(data => data.TotalVolume),
                TotalVolume = group.Sum(data => data.TotalVolume)
            }).ToList();
        return combinedData;
    }
}