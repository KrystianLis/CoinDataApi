using Microsoft.AspNetCore.Mvc;

namespace CoinDataApi.Web.Query;

public record DateRangeQuery([FromQuery]string? TimeStart, [FromQuery]string? TimeEnd);