using Application.Weather.Commands.AddWeather;
using Application.Weather.Queries.GetWeather;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

       
        [Authorize]
        [HttpGet(Name = "weathr-forecast/all")]
        public async Task<List<WeatherForecast>> GetAll()
        {
            return await Mediator.Send(new GetallWeatherForecastQuery());
        }

        //[HttpGet(Name = "/weathr-forecast/free")]
        //public async Task<List<WeatherForecast>> GetAllFree()
        //{
        //    return await Mediator.Send(new GetallWeatherForecastQuery());
        //}

        [HttpPost(Name = "weathr-forecast")]
        public async Task<WeatherForecast> AddNew([FromBody] AddWeatherCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
