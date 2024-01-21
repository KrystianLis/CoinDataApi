using CoinDataApi.Core.Interfaces.Clients;
using CoinDataApi.Infrastructure;
using CoinDataApi.Infrastructure.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ICoinApiClient, CoinApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CoinApi:Url")!);
});

builder.Services.Configure<CoinApiOptions>(builder.Configuration.GetRequiredSection("CoinApi"));

var app = builder.Build();

app.MapGet("/execute", async ([FromServices] ICoinApiClient dataService, CancellationToken token) =>
{
    var entries = await dataService.GetOhlcvFromLastDay("bitstamp_spot_btc_usd", token: token);
    return Results.Ok(entries);
}).WithName("Get Data");

app.Run();