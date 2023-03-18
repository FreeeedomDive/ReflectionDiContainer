namespace ReflectionDiContainer.Tests.Api.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetWeather();
}