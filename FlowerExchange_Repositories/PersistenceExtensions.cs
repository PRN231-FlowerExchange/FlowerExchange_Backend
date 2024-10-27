using Domain.Commons.BaseRepositories;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using Persistence.Interceptors;
using Persistence.RepositoryAdapter;

namespace Microsoft.Extensions.DependencyInjection;


public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, string connectionString, string migrationsAssembly = "")
    {
        services.AddDbContext<FlowerExchangeDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, sql =>
           {
               if (!string.IsNullOrEmpty(migrationsAssembly))
               {
                   sql.MigrationsAssembly(migrationsAssembly);
               }
           });

            options.UseLazyLoadingProxies();
            // Add the interceptor to the DbContext
            options.AddInterceptors(serviceProvider.GetServices<AuditableEntityInterceptor>());
        })
            .AddDbContext<FlowerExchangeDbContext>((Action<DbContextOptionsBuilder>)null, ServiceLifetime.Scoped)
            .AddRepositories();

        services.AddScoped<FlowerExchangeDbContextInitialiser>();



        return services;
    }


    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>))
                .AddScoped(typeof(IWeatherForecastRepository), typeof(WeatherForecastRepository))
                .AddScoped(typeof(IPostRepository), typeof(PostRepossitory))
                .AddScoped(typeof(IPostServiceRepository), typeof(PostServiceRepository))
                .AddScoped(typeof(IServiceRepository), typeof(ServiceRepository))
                .AddScoped(typeof(IUserRepository), typeof(UserRepository))
                .AddScoped<IPostRepository, PostRepossitory>()
                .AddScoped<IWalletTransactionRepository, WalletTransactionRepository>()
                .AddScoped(typeof(IFlowerRepository), typeof(FlowerRepository))
                .AddScoped(typeof(IConversationRepository), typeof(ConversationRepository))
                .AddScoped(typeof(IMessageRepository), typeof(MessageRepository))
                .AddScoped(typeof(IUserConversationRepository), typeof(UserConversationRepository))
                .AddScoped(typeof(IWalletRepository), typeof(WalletRepository))
                .AddScoped(typeof(ITransactionRepository), typeof(TransactionRepository))
                .AddScoped(typeof(IServiceOrderRepository), typeof(ServiceOrderRepository))
                .AddScoped(typeof(ICategoriesRepository), typeof(CategoryRepository))
                .AddScoped(typeof(IStoreRepository), typeof(StoreRepository))
                .AddScoped(typeof(IWalletRepository), typeof(WalletRepository))
                .AddScoped(typeof(IFlowerOrderRepository), typeof(FlowerOrderRepository));


        //.AddScoped(typeof(IUserRepository), typeof(UserRepository))
        //.AddScoped(typeof(IRoleRepository), typeof(RoleRepository))



        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        //var assembly = Assembly.GetExecutingAssembly(); // or specify the assembly if different

        ////Register all repository implementing IRepository<>
        //services.Scan(scan => scan
        //    .FromAssemblies(typeof(IRepositoryBase<,>).Assembly)// Scan the assembly where RepositoryBase is located
        //    .AddClasses(classes => classes.AssignableTo(typeof(IRepositoryBase<,>))) // Add all classes that implement IRepositoryBase<>
        //    .UsingRegistrationStrategy(RegistrationStrategy.Skip) // Skip registration if already registered
        //    .AsImplementedInterfaces() // Register as the interfaces they implement
        //    .WithScopedLifetime()); // Set the lifetime for the repository

        return services;
    }
}

