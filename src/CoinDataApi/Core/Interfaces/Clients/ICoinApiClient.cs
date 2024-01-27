using CoinDataApi.Core.Models;

namespace CoinDataApi.Core.Interfaces.Clients;

public interface ICoinApiClient
{
    Task<IReadOnlyCollection<OhlcvData>> GetHistoricalData(string symbolId, string timeStart, string timeEnd, string periodId = "1DAY", bool includeEmptyItems = false, CancellationToken token = default);
}