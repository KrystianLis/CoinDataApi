using System.Text.Json.Serialization;

namespace CoinDataApi.Application.DTOs;

public record PriceAnalysisResultDto
{
    [JsonPropertyName("vwap_24h")]
    public decimal Vwap24H { get; set; }
    
    [JsonPropertyName("standard_deviation_24h")]
    public double StandardDeviation24H { get; set; }
    
    [JsonPropertyName("points_with_high_deviation_24h")]
    public IEnumerable<OhlcvDataDto> PointsWithHighDeviation24H { get; set; } = null!;

    [JsonPropertyName("vwap")]
    public decimal Vwap { get; set; }

    [JsonPropertyName("standard_deviation")]
    public double StandardDeviation { get; set; }

    [JsonPropertyName("points_with_high_deviation")]
    public IEnumerable<OhlcvDataDto> PointsWithHighDeviation { get; set; } = null!;
}