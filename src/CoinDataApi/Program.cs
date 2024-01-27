using CoinDataApi.Application;
using CoinDataApi.Application.Services;
using CoinDataApi.Infrastructure;
using CoinDataApi.Infrastructure.Errors;
using CoinDataApi.Web.Query;
using CoinDataApi.Web.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services
    .AddErrorHandler();

builder.Services.AddScoped<IValidator<DateRangeQuery>, DateRangeQueryValidator>();

var app = builder.Build();

app.UseErrorHandler();

app.MapGet("/execute", async ([AsParameters]DateRangeQuery query, IValidator<DateRangeQuery> validator, [FromServices] IDataAggregatorService dataService, CancellationToken token) =>
{
    var validationResult = await validator.ValidateAsync(query, token);

    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.ToDictionary());
    }
    
    var points = await dataService.AggregateDataAsync(query.TimeStart!, query.TimeEnd!, query.PeriodId!, token);
    return Results.Ok(points);
}).WithName("Get Data");

app.Run();