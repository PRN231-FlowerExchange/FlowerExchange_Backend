using Domain.Entities;
using Domain.Security.Identity;
using IdentityModel.Client;
using Infrastructure.Security.Identity;
using Infrastructure.Security.TokenProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Security;

//List the collection of IdentityServiceCollection for later add to program as option configuration
public static class IdentityServiceCollectionExtensions
{
    private const string MyNewEmailTokenProviderName = "EmailConfirmation";

    public static IServiceCollection AddAutheticationService(this IServiceCollection services, IConfiguration configuration)
    {
        string clientId = configuration["Authentication:LoginPurpose:Google:ClientId"] ?? throw new ArgumentNullException("Null ClientId");

        string clientSecret = configuration["Authentication:LoginPurpose:Google:ClientSecret"] ?? throw new ArgumentNullException("Null ClientSecret");
        Console.WriteLine("Client ID: " + clientId);
        Console.WriteLine("Client Secret: " + clientSecret);

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
                .AddJwtBearer()
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
                        Console.WriteLine("\n==== EVENT DONE GOOGLE REDIRECTION - GOOGLE ON CREATING TICKET ====");
                        Console.WriteLine("\nToken Google Endpoint: " + options.TokenEndpoint //The google handler will automatically request the token from the token endpoint by using the code, state, clientId, ClientSecret, RedirectUri
                                        + "\nAuthorization Endpoint: " + options.AuthorizationEndpoint
                                        + "\nUserInformation Endpoint: " + options.UserInformationEndpoint);
                        Console.WriteLine("\n---> ClaimActions");
                        foreach (var s in options.ClaimActions)
                        {
                            Console.WriteLine("Claim action type: " + s.ClaimType);
                        }
                        Console.WriteLine("\nAccessToken: " + context.AccessToken);
                        Console.WriteLine("\nRefreshToken: " + context.RefreshToken);
                        Console.WriteLine("\nTokenResponse: " + context.TokenResponse + " Context Token Type: " + context.TokenType);
                        Console.WriteLine("\nContext User: " + context.User.GetRawText());
                        Console.WriteLine("\n---> Request Query Values: ");
                        foreach (var s in context.HttpContext.Request.Query)
                        {
                            Console.WriteLine("Query Key: " + s.Key + " - Value: " + s.Value);
                        }
                        Console.WriteLine("\n---> User Claims");
                        foreach (var s in context.User.ToClaims())
                        {
                            Console.WriteLine("Claim type: " + s.Type + " - Value: " + s.Value);
                        }

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
        //AddDefaultTokenProviders : Adds the default token providers used to generate tokens for reset passwords,
        //     change email and change telephone number operations, and for two factor authentication
        //     token generation.

        //EmailConfirmationTokenProvider: A custom email token provider generate toke for email confirmation

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
            options.TokenLifespan = TimeSpan.FromHours(tokenLifespan);
        });

        services.Configure<EmailConfirmationTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(emailTokenLifespan);
        });


        services.Configure<IdentityOptions>(options =>
        {
            options.Tokens.EmailConfirmationTokenProvider = MyNewEmailTokenProviderName;

            //Default Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(defaultLockoutTimeSpan);
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;


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

            options.Tokens.EmailConfirmationTokenProvider = MyNewEmailTokenProviderName; //or you default
        });
    }




}

