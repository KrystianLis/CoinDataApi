namespace CoinDataApi.Application.Services;

public interface IService
{
    public Task<string> AggregateDataAsync(CancellationToken token = default);
}