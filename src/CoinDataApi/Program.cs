using CoinDataApi.Application;
using CoinDataApi.Application.Services;
using CoinDataApi.Infrastructure;
using CoinDataApi.Infrastructure.Errors;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services
    .AddErrorHandler();

var app = builder.Build();

app.UseErrorHandler();

app.MapGet("/execute", async ([FromQuery(Name = "time_start")] string timeStart, [FromQuery(Name = "time_end")] string timeEnd, [FromServices] IDataAggregatorService dataService, CancellationToken token) =>
{
    if (!DateTime.TryParse(timeStart, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var startTimeUtc))
    {
        return Results.BadRequest("Invalid timeStart format. Please use ISO 8601 format in UTC.");
    }

    if (!DateTime.TryParse(timeEnd, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var endTimeUtc))
    {
        return Results.BadRequest("Invalid timeEnd format. Please use ISO 8601 format in UTC.");
    }
    
    if (endTimeUtc < startTimeUtc)
    {
        return Results.BadRequest("time_end must not be earlier than time_start.");
    }
    
    var points = await dataService.AggregateDataAsync(timeStart, timeEnd, token);
    return Results.Ok(points);
}).WithName("Get Data");

app.Run();