using Domain.Entities;
using Domain.Security.Identity;
using IdentityModel.Client;
using Infrastructure.Security.Identity;
using Infrastructure.Security.TokenProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            // Không tự động trả về lỗi 403
                            context.HandleResponse();

                            // Trả về lỗi 401 Unauthorized khi không có JWT hoặc không hợp lệ
                            throw new UnauthorizedAccessException("Unauthorized. You need to log in to access this resource.");
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

