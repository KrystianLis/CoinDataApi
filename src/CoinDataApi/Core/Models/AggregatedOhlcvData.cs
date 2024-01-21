namespace CoinDataApi.Core.Models;

public class AggregatedOhlcvData
{
    public DateTime PeriodStart { get; set; }
    public decimal PriceCloseWeighted { get; set; }
    public decimal TotalVolume { get; set; }
}