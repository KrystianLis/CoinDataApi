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

    public async Task<PriceAnalysisResultDto> AggregateDataAsync(string timeStart, string timeEnd, CancellationToken token = default)
    {
        // Due to the CoinApi restrictions, it is not possible to use Task.WhenAll()
        var bitstampData = await _client.GetHistoricalData("bitstamp_spot_btc_usd", timeStart, timeEnd, token: token);
        var coinbaseData = await _client.GetHistoricalData("coinbase_spot_btc_usd", timeStart, timeEnd, token: token);

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

        var vwap = VwapCalculator.CalculateVwap(combinedData);
        var standardDeviation = VwapCalculator.CalculateStandardDeviation(combinedData, vwap);
        
        var pointsWithHighDeviation = VwapCalculator.FindPointsWithHighDeviation(combinedData, vwap, standardDeviation);
        
        return new PriceAnalysisResultDto
        {
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
}