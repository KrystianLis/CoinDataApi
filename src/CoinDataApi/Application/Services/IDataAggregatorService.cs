using CoinDataApi.Core.Models;

namespace CoinDataApi.Application.Services;

public interface IDataAggregatorService
{
    public Task<IEnumerable<OhlcvData>> AggregateDataAsync(string timeStart, string timeEnd, CancellationToken token = default);
}