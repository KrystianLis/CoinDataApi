using CoinDataApi.Application.DTOs;

namespace CoinDataApi.Application.Services;

public interface IDataAggregatorService
{
    public Task<PriceAnalysisResultDto> AggregateDataAsync(string timeStart, string timeEnd, CancellationToken token = default);
}