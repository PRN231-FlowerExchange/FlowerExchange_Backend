using Application.Common.Behaviors;
using Application.Services.EmailForIdentityService;
using Application.UserIdentity.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Application;

public static class ApplicationExtenstions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register AutoMapper with all profiles in the assembly
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register MediatR and all handlers in the assembly
        services.AddMediatR(ctg =>
        {
            ctg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //validation
            ctg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        });
        // Register pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<EmailForIdentityService>();
        services.AddScoped<TokenFactory>();

        return services;
    }

}

