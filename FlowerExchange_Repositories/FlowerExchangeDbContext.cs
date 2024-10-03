using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Persistence
{
    public partial class FlowerExchangeDbContext : IdentityDbContext<User, Role, Guid>
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
            //.UseNpgsql(this.GetConnectionString());
            .UseNpgsql("Host=localhost; Database=flowerexchangedb; Username=postgres; Password=hanh3533.");

        }


        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var connection = configuration.GetConnectionString("FlowerExchangeDB");
            if(connection == null)
            {
                throw new ArgumentNullException("CONNECTION IS NULL");
            }
            Console.WriteLine(connection + "DAY");
            return connection;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Role { get; set; }

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
            EntityIdentityConfiguration(modelBuilder);            

        }

        private void EntityIdentityConfiguration(ModelBuilder builder)
        {


            builder.Entity<IdentityUserClaim<Guid>>(uc =>
                uc.ToTable("UserClaim")
                   .HasKey(c => c.Id)
            ); 

            builder.Entity<IdentityUserToken<Guid>>(ut =>
                ut.ToTable("UserToke")
                  .HasKey(t => new { t.UserId, t.LoginProvider, t.Name })
            );

            builder.Entity<IdentityUserLogin<Guid>>(ul =>
                ul.ToTable("UserLogin")
                  .HasKey(l => new { l.LoginProvider, l.ProviderKey })
            );

            builder.Entity<IdentityUserRole<Guid>>(ur =>
            {
                ur.ToTable("UserRole")
                  .HasKey(r => new { r.UserId, r.RoleId });
            });


            builder.Entity<IdentityRoleClaim<Guid>>(rl =>
                rl.ToTable("RoleClaim")
                  .HasKey(rc => rc.Id)
            );

        }

    }
}
