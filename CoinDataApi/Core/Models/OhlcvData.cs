using Newtonsoft.Json;

namespace CoinDataApi.Core.Models;

public class OhlcvData
{
    [JsonProperty("price_close")]
    public decimal PriceClose { get; set; }

    [JsonProperty("volume_traded")]
    public decimal VolumeTraded { get; set; }
    
    [JsonProperty("time_period_start")]
    public DateTime TimePeriodStart { get; set; }
}