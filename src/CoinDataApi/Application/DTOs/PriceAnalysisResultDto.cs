using System.Text.Json.Serialization;

namespace CoinDataApi.Application.DTOs;

public record PriceAnalysisResultDto
{
    public decimal Vwap { get; set; }

    [JsonPropertyName("standard_deviation")]
    public double StandardDeviation { get; set; }

    [JsonPropertyName("points_with_high_deviation")]
    public IEnumerable<OhlcvDataDto> PointsWithHighDeviation { get; set; } = null!;
}