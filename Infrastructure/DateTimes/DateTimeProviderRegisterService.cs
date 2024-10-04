using CrossCuttingConcerns.Datetimes;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DateTimes;

public static class DateTimeProviderRegisterService
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        _ = services.AddSingleton<IDateTimeProvider, DateTmeProvider>();
        return services;
    }
}


