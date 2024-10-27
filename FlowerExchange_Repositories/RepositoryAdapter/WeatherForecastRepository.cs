using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;

namespace Persistence.RepositoryAdapter
{
    public class WeatherForecastRepository : RepositoryBase<WeatherForecast, Guid>, IWeatherForecastRepository
    {
        public WeatherForecastRepository(IUnitOfWork<FlowerExchangeDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
