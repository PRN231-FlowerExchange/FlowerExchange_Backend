using Domain.Entities;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Weather.Queries.GetWeather
{
    public class GetallWeatherForecastQuery : IRequest<List<WeatherForecast>>
    {

    }

    public class GetallWeatherForecastQueryHandler : IRequestHandler<GetallWeatherForecastQuery, List<WeatherForecast>>
    {
        private IWeatherForecastRepository _weatherRepository;

        private readonly ILogger<GetallWeatherForecastQueryHandler> _logger;

        public GetallWeatherForecastQueryHandler(IWeatherForecastRepository weatherRepository, ILogger<GetallWeatherForecastQueryHandler> logger)
        {
            _weatherRepository = weatherRepository;
            _logger = logger;
        }

        public async Task<List<WeatherForecast>> Handle(GetallWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<WeatherForecast> list =  await _weatherRepository.GetAllAsync();
            
            return list.ToList();
        }
    }
}
