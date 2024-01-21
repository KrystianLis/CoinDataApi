using System.Net.Http.Headers;
using CoinDataApi.Core.Interfaces.Clients;
using CoinDataApi.Core.Models;
using Microsoft.Extensions.Options;

namespace CoinDataApi.Infrastructure.Clients;

public class CoinApiClient : ICoinApiClient
{
    private readonly HttpClient _httpClient;

    public CoinApiClient(HttpClient httpClient, IOptions<CoinApiOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("X-CoinAPI-Key", options.Value.Key);
    }

    public async Task<IReadOnlyCollection<OhlcvData>> GetOhlcvFromLastDay(string symbolId,
        bool includeEmptyItems = false, CancellationToken token = default)
    {
        return (await _httpClient.GetFromJsonAsync<IReadOnlyCollection<OhlcvData>>(
            $"/v1/ohlcv/{symbolId}/latest?period_id=1DAY&include_empty_items={includeEmptyItems}", token))!;
    }
}