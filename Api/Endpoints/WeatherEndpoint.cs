﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public class WeatherEndpoints : IEndpointBuilder
{
    private static readonly string[] summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

    public static IEndpointRouteBuilder RegisterEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weatherforecast/{test}/hallo")
            .WithTags("WeatherForecast is here")
            .AddEndpointFilter(async (context, next) =>
            {
                var result = await next(context);
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
                {

                }
                return result;
            }); ;

        group
            .MapGet("/{hoi}", GetForecast)
            .WithName(nameof(GetForecast));

        group
            .MapPost("/", PostForecast)
            .WithName(nameof(PostForecast));

        return app;
    }

    private static Results<Ok<WeatherForecast[]>, NotFound> GetForecast(IConfiguration configuration, int test, int hoi)
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();

        return TypedResults.Ok(forecast);
    }

    private static Results<Ok<WeatherForecast>, NotFound> PostForecast([FromBody] WeatherForecast weatherForecast, int test)
    {
        return TypedResults.Ok(weatherForecast);
    }
}
