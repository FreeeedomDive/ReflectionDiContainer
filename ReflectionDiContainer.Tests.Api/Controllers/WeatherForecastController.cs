using Microsoft.AspNetCore.Mvc;
using ReflectionDiContainer.Tests.Api.Services;

namespace ReflectionDiContainer.Tests.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService weatherService;

    public WeatherForecastController(IWeatherService weatherService)
    {
        this.weatherService = weatherService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return weatherService.GetWeather();
    }
}