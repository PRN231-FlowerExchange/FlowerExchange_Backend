using Application.Weather.Commands.AddWeather;
using Application.Weather.Queries.GetWeather;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : APIControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "weathr-forecast/all")]
        public async Task<List<WeatherForecast>> GetAll()
        {
            return await Mediator.Send(new GetallWeatherForecastQuery());
        }

        [HttpPost(Name = "weathr-forecast")]
        public async Task<WeatherForecast> AddNew([FromBody] AddWeatherCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
