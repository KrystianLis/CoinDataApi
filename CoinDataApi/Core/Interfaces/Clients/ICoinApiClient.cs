using CoinDataApi.Core.Models;

namespace CoinDataApi.Core.Interfaces.Clients;

public interface ICoinApiClient
{
    Task<IReadOnlyCollection<OhlcvData>> GetOhlcvFromLastDay(string symbolId, bool includeEmptyItems = false, CancellationToken token = default);
}