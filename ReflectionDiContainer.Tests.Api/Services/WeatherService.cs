namespace ReflectionDiContainer.Tests.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly ILog<WeatherService> log;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherService(ILog<WeatherService> log)
    {
        this.log = log;
    }

    public IEnumerable<WeatherForecast> GetWeather()
    {
        log.Info($"{nameof(GetWeather)}");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}