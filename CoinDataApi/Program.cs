using CoinDataApi.Core.Interfaces.Clients;
using CoinDataApi.Infrastructure;
using CoinDataApi.Infrastructure.Errors;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddErrorHandler();

var app = builder.Build();

app.UseErrorHandler();

app.MapGet("/execute", async ([FromServices] ICoinApiClient dataService, CancellationToken token) =>
{
    var entries = await dataService.GetOhlcvFromLastDay("bitstamp_spot_btc_usd", token: token);
    return Results.Ok(entries);
}).WithName("Get Data");

app.Run();