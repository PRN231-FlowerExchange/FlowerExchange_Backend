using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.RepositoryAdapter;
using System.Reflection;


namespace Persistence
{
    public partial class FlowerExchangeDbContext : DbContext
    {

        public FlowerExchangeDbContext()
        {
        }

        public FlowerExchangeDbContext(DbContextOptions<FlowerExchangeDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            //.UseLazyLoadingProxies()
            .UseNpgsql(this.GetConnectionString());

        }


        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var connection = configuration.GetConnectionString("FlowerExchangeDB");
            Console.WriteLine(connection + "DAY");
            return connection;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<UserConversation> UserConversations { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<Flower> Flowers { get; set; }

        public DbSet<FlowerOrder> FlowerOrders { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<PostService> PostServices { get; set; }

        public DbSet<ServiceOrder> ServiceOrders { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        //public DbSet<DepositTransaction> DepositTransactions { get; set; }

        public DbSet<WeatherForecast> WeatherForecast { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          

        }

    }
}
