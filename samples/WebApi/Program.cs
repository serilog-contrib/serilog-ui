using Serilog.Ui.Web.Extensions;
using WebApi.Extensions;
using WebApi.HostedServices;

var builder = WebApplication.CreateBuilder(args);

var enableElasticSample = builder.Configuration.GetSection("SerilogUi").GetValue<bool>("EnableElasticServices");
builder.Host.ConfigureSerilog(enableElasticSample);

builder.Services
    .AddHostedService<SqlServerContainerService>()
    .AddEndpointsApiExplorer()
    .AddSerilogUiSample(builder.Configuration, enableElasticSample)
    .AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogUi(options =>
{
    options.RoutePrefix = "my-test";
    options.HideBrand = true;
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}