using CoinDataApi.Application;
using CoinDataApi.Application.Services;
using CoinDataApi.Core.Interfaces.Clients;
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

app.MapGet("/execute", async ([FromServices] IService dataService, CancellationToken token) =>
{
    var entries = await dataService.AggregateDataAsync();
    return Results.Ok(entries);
}).WithName("Get Data");

app.Run();