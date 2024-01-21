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

app.MapGet("/execute", async ([FromServices] IDataAggregatorService dataService, CancellationToken token) =>
{
    var points = await dataService.AggregateDataAsync(token);
    return Results.Ok(points);
}).WithName("Get Data");

app.Run();