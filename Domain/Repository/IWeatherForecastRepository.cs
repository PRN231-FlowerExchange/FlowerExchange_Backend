using Domain.Commons.BaseRepositories;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IWeatherForecastRepository : IRepositoryBase<WeatherForecast, Guid>
    {
    }
}
