namespace CoinDataApi.Application.Services;

public interface IDataAggregatorService
{
    public Task<string> AggregateDataAsync(CancellationToken token = default);
}