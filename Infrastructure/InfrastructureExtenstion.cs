using Application.Services.JwtTokenService;
using Domain.EmailProvider;
using Infrastructure.DateTimes;
using Infrastructure.EmailProvider.Gmail;
using Infrastructure.Security;
using Infrastructure.Security.TokenProvider;
using Microsoft.Extensions.Configuration;


namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureExtenstion
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDateTimeProvider();
        services.AddScoped<IEmailProvider, GmailProvider>();
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddIdentityConfiguration();
        services.AddAutheticationService(configuration);


        return services;
    }
}

