using System.Text.Json.Serialization;

namespace CoinDataApi.Core.Models;

public class OhlcvData
{
    [JsonPropertyName("price_close")]
    public decimal ClosePrice { get; set; }

    [JsonPropertyName("volume_traded")]
    public decimal TotalVolume { get; set; }
    
    [JsonPropertyName("time_period_start")]
    public DateTime TimePeriodStart { get; set; }
    
    [JsonPropertyName("time_period_end")]
    public DateTime TimePeriodEnd { get; set; }
}