using System.Net.Http.Headers;
using System.Text.Json;
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

    public async Task<IReadOnlyCollection<OhlcvData>> GetHistoricalData(string symbolId, string timeStart, string timeEnd,
        string periodId = "1DAY", bool includeEmptyItems = false, CancellationToken token = default)
    {
        var httpResponseMessage = await _httpClient.GetAsync(
            $"/v1/ohlcv/{symbolId}/history?period_id={periodId}&time_start={timeStart}&time_end={timeEnd}&include_empty_items={includeEmptyItems}", token);

        var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync(token);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseMessage);
        }

        return JsonSerializer.Deserialize<IReadOnlyCollection<OhlcvData>>(responseMessage)!;
    }
    
    public async Task<IReadOnlyCollection<OhlcvData>> Get24hLatestData(string symbolId, bool includeEmptyItems = false, CancellationToken token = default)
    {
        var httpResponseMessage = await _httpClient.GetAsync(
            $"/v1/ohlcv/{symbolId}/latest?period_id=1HRS&limit=24&include_empty_items={includeEmptyItems}", token);

        var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync(token);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseMessage);
        }

        return JsonSerializer.Deserialize<IReadOnlyCollection<OhlcvData>>(responseMessage)!;
    }
}