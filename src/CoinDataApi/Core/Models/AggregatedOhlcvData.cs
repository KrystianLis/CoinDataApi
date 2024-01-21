namespace CoinDataApi.Core.Models;

public class AggregatedOhlcvData
{
    public DateTime PeriodStart { get; set; }
    public decimal ClosePrice { get; set; }
    public decimal TotalVolume { get; set; }
}