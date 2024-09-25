using CrossCuttingConcerns.Datetimes;
using Infrastructure.DateTimes;




namespace  Microsoft.Extensions.DependencyInjection;

public static class DateTimeProviderRegisterService
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        _ = services.AddSingleton<IDateTimeProvider, DateTmeProvider>();
        return services;
    }
}


