using Castle.Core.Configuration;
using Domain.EmailProvider;
using Domain.Entities;
using Domain.Security.JwtTokenService;
using Infrastructure.DateTimes;
using Infrastructure.EmailProvider.Gmail;
using Infrastructure.Security;
using Infrastructure.Security.TokenProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureExtenstion
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDateTimeProvider();
        services.AddScoped<IEmailProvider, GmailProvider>();
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddIdentityConfiguration();


        return services;
    }
}

