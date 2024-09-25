
using Application.Weather.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Persistence;


namespace Application.Weather.Commands.AddWeather
{
    public record AddWeatherCommand : IRequest<WeatherForecast>
    {
        public WeatherForecastDTO WeatherForecastDTO { get; init; } = default;
    }

    public class AddWeatherCommandHandler : IRequestHandler<AddWeatherCommand, WeatherForecast>
    {
        private IMapper _mapper;
        private IWeatherForecastRepository _weatherForecastRepository;
        private IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public AddWeatherCommandHandler(IMapper mapper, IWeatherForecastRepository weatherForecastRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _weatherForecastRepository = weatherForecastRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<WeatherForecast> Handle(AddWeatherCommand request, CancellationToken cancellationToken)
        {
            WeatherForecast entity = _mapper.Map<WeatherForecast>(request.WeatherForecastDTO);
            Console.WriteLine($"Entity State Before Save: {_unitOfWork.Context.Entry(entity).State}");
            await _weatherForecastRepository.InsertAsync(entity);
            Console.WriteLine($"Entity State Before Save: {_unitOfWork.Context.Entry(entity).State}");
            await _unitOfWork.SaveChangesAsync();

            WeatherForecast savedEntity = await _weatherForecastRepository.GetByIdAsync(entity.Id);
            return savedEntity;
        }
    }

}
