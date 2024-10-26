using Application.Weather.Commands.AddWeather;
using Application.Weather.Queries.GetWeather;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("weather-forecast")]
    public class WeatherForecastController : APIControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [Authorize]
        [HttpGet("require-authorize")]
        public async Task<List<WeatherForecast>> GetAll()
        {
            return await Mediator.Send(new GetallWeatherForecastQuery());
        }

        [HttpGet("no-authorize")]
        public async Task<List<WeatherForecast>> GetAllFree()
        {
            return await Mediator.Send(new GetallWeatherForecastQuery());
        }

        [HttpPost]
        public async Task<WeatherForecast> AddNew([FromBody] AddWeatherCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
