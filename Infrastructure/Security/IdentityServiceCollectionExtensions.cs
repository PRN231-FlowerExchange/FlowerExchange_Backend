﻿using Domain.Entities;
using Domain.Security.Identity;
using Infrastructure.Security.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using System;
using System.Net;
using System.Net.Http;


namespace Infrastructure.Security;

//List the collection of IdentityServiceCollection for later add to program as option configuration
public static class IdentityServiceCollectionExtensions
{
    private const string MyNewEmailTokenProviderName = "EmailConfirmation";

    public static IServiceCollection AddAutheticationService(this IServiceCollection services, IConfiguration configuration)
    {
        string clientId = configuration["Authentication:LoginPurpose:Google:ClientId"] ?? throw new ArgumentNullException("Null ClientId");

        string clientSecret = configuration["Authentication:LoginPurpose:Google:ClientSecret"] ?? throw new ArgumentNullException("Null ClientSecret");

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse(); // Prevent default behavior
                            var response = context.Response;
                            var problemDetails = new ProblemDetails
                            {
                                Detail = "You are not authorized to access this resource. Please provide a valid token.",
                                Instance = null,
                                Status = (int)HttpStatusCode.Unauthorized,
                                Title = "Unauthorized",
                                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.5"
                            };

                            problemDetails.Extensions.Add("message", "You are not authorized to access this resource. Please provide a valid token.");

                            response.ContentType = "application/problem+json";
                            response.StatusCode = problemDetails.Status.Value;

                            await response.WriteAsJsonAsync(problemDetails);
                        },
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsJsonAsync(new
                            {
                                error = "You do not have permission to access this resource."
                            });

                            var response = context.Response;
                            var problemDetails = new ProblemDetails
                            {
                                Detail = "You do not have permission to access this resource.",
                                Instance = null,
                                Status = (int)HttpStatusCode.Forbidden,
                                Title = "Forbidden",
                                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3"
                            };

                            problemDetails.Extensions.Add("message", "You are not authorized to access this resource. Please provide a valid token.");

                            response.ContentType = "application/problem+json";
                            response.StatusCode = problemDetails.Status.Value;

                            await response.WriteAsJsonAsync(problemDetails);
                        }
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:LoginPurpose:Google:ClientId"];
                    options.ClientSecret = configuration["Authentication:LoginPurpose:Google:ClientSecret"];
                    options.ClaimActions.MapJsonKey("picture", "picture");
                    options.SaveTokens = true;
                    options.CallbackPath = "/api/account/signin-google";
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.Events.OnCreatingTicket = (context) =>
                    {
                        return Task.CompletedTask;
                    };                    
  
                });
        return services;
    }


    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentityCore<User>()
                 .AddRoles<Role>()
                 .AddEntityFrameworkStores<FlowerExchangeDbContext>()
                 .AddApiEndpoints()
                 .AddPasswordValidators()
                 .AddTokenProviders();
        services.AddIdentityApiEndpoints<User>();


        IdentityConfigurationOptions(services);

        services.AddHttpContextAccessor();

        // Register CurrentWebUser as ICurrentUser
        services.AddScoped<ICurrentUser, CurrentWebUser>();

        return services;

    }


    private static IdentityBuilder AddTokenProviders(this IdentityBuilder identityBuilder)
    {

        identityBuilder
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailConfirmationTokenProvider<User>>(MyNewEmailTokenProviderName);

        return identityBuilder;
    }

    private static IdentityBuilder AddPasswordValidators(this IdentityBuilder identityBuilder)
    {
        identityBuilder
            .AddPasswordValidator<HistoricalPasswordValidator>()
            .AddPasswordValidator<WeakPasswordValidator>();

        return identityBuilder;
    }

    private static void IdentityConfigurationOptions(IServiceCollection services)
    {
        int tokenLifespan = 3; //hours
        int emailTokenLifespan = 15; //minutes
        int defaultLockoutTimeSpan = 5; //minutes
        int defaultMaxfailedAccessAttempts = 3; //times


        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(emailTokenLifespan);
        });

        services.Configure<EmailConfirmationTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(emailTokenLifespan);
        });



        services.Configure<IdentityOptions>(options =>
        {
            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
            //Default Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(defaultLockoutTimeSpan);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;
            options.Tokens.ProviderMap.Add("CustomEmailConfirmation", new TokenProviderDescriptor(
                typeof(EmailConfirmationTokenProvider<User>)));
            options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
        });



        services.Configure<PasswordOptions>(options =>
        {
            options.RequireDigit = true;
            options.RequireLowercase = true;
            options.RequireNonAlphanumeric = true;
            options.RequireUppercase = true;
            options.RequiredLength = 8;
            options.RequiredUniqueChars = 1;

            //OR you custom additional password validation through WeakPasswordValidator class 
        });

        //The default ASP.NET Core Identity password hasher uses PBKDF2 with HMAC - SHA256, a 128 - bit salt, a 256 - bit subkey, and 10,000 iterations.
        //You can consider to implement new password hasher like Argon2, Bcrypt, Scrypt, ....
        //Follow the instruction https://andrewlock.net/safely-migrating-passwords-in-asp-net-core-identity-with-a-custom-passwordhasher/
        services.Configure<PasswordHasherOptions>(optiosn =>
        {
            optiosn.IterationCount = 1000;
            optiosn.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
        });

        services.Configure<IdentityOptions>(options =>
        {
            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;

        });


    }




}

