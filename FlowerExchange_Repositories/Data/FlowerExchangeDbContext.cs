using FlowerExchange_Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerExchange_Repositories.Data
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
            => optionsBuilder
            .UseNpgsql("Host=ep-cold-forest-a4tdd8ig-pooler.us-east-1.aws.neon.tech; Database=flowerexchangedb; Username=default; Password=Ym1Oz8GbMsEI");

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:FlowerExchangeDB"];
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

        public DbSet<DepositTransaction> DepositTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // User one to one Store
            modelBuilder.Entity<User>()
                .HasOne(u => u.Store)
                .WithOne(s => s.Owner)
                .HasForeignKey<Store>(s => s.OwnerId);

            // Post many to many Category => PostCategory
            modelBuilder.Entity<PostCategory>()
                .HasKey(pc => new { pc.PostId, pc.CategoryId });
            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.PostId);
            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(pc => pc.CategoryId);

            // Post one to many Report 
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => r.PostId);

            // User one to many Post
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Seller)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.SellerId);

            // Store one to many Post
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Store)
                .WithMany(s => s.Posts)
                .HasForeignKey(p => p.StoreId);

            // User one to one Wallet
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId);

            // Wallet one to many DepositTransaction
            modelBuilder.Entity<DepositTransaction>()
                .HasOne(dt => dt.Wallet)
                .WithMany(w => w.DepositTransactions)
                .HasForeignKey(dt => dt.WalletId);

            // Post one to one Flower
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Flower)
                .WithOne(f => f.Post)
                .HasForeignKey<Flower>(f => f.PostId);

            // Flower one to one FlowerOrder
            modelBuilder.Entity<Flower>()
                .HasOne(f => f.FlowerOrder)
                .WithOne(fo => fo.Flower)
                .HasForeignKey<FlowerOrder>(fo => fo.FlowerId);

            // FlowerOrder one to many Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FlowerOrder)
                .WithMany(fo => fo.Transactions)
                .HasForeignKey(t => t.FlowerOrderId);

            // Post many to many Service => PostService
            modelBuilder.Entity<PostService>()
                .HasOne(ps => ps.Post)
                .WithMany(p => p.PostServices)
                .HasForeignKey(ps => ps.PostId);
            modelBuilder.Entity<PostService>()
                .HasOne(ps => ps.Service)
                .WithMany(s => s.PostServices)
                .HasForeignKey(ps => ps.ServiceId);

            // ServiceOrder one to many PostService
            modelBuilder.Entity<PostService>()
                .HasOne(po => po.ServiceOrder)
                .WithMany(so => so.PostServices)
                .HasForeignKey(po => po.ServiceId);

            // ServiceOrder one to many Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ServiceOrder)
                .WithMany(so => so.Transactions)
                .HasForeignKey(t => t.ServiceOrderId);

            // Wallet many to many Transaction => WalletTransaction
            modelBuilder.Entity<WalletTransaction>()
                .HasKey(wt => new { wt.WalletId, wt.TransactonId });
            modelBuilder.Entity<WalletTransaction>()
                .HasOne(wt => wt.Wallet)
                .WithMany(w => w.WalletTransactions)
                .HasForeignKey(wt => wt.WalletId);
            modelBuilder.Entity<WalletTransaction>()
                .HasOne(wt => wt.Transaction)
                .WithMany(t => t.WalletTransactions)
                .HasForeignKey(wt => wt.TransactonId);

            // User many to many Conversation
            modelBuilder.Entity<UserConversation>()
                .HasKey(uc => new { uc.UserId, uc.ConversationId });
            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserConversations)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserConversation>()
                .HasOne(uc => uc.Conversation)
                .WithMany(c => c.UserConversations)
                .HasForeignKey(uc => uc.ConversationId);

            //Message one to many User and Conversation
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(s => s.Messages)
                .HasForeignKey(m => m.SenderId);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);

            // Buyer (User) one to many ServiceOrder
            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Buyer)
                .WithMany(b => b.ServiceOrders)
                .HasForeignKey(so => so.BuyerId);

            // Buyer (User) one to many FlowerOrder
            modelBuilder.Entity<FlowerOrder>()
                .HasOne(fo => fo.Buyer)
                .WithMany(b => b.BuyOrders)
                .HasForeignKey(fo => fo.BuyerId);

            // Seller (User) one to many FlowerOrder
            modelBuilder.Entity<FlowerOrder>()
                .HasOne(fo => fo.Seller)
                .WithMany(s => s.SellOrders)
                .HasForeignKey(fo => fo.SellerId);

            // User one to many Report (Create)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.CreateBy)
                .WithMany(cb => cb.CreatedReports)
                .HasForeignKey(r => r.CreateById);

            // User one to many Report (Update)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.UpdateBy)
                .WithMany(ub => ub.UpdatedReports)
                .HasForeignKey(r => r.UpdateById);

        }
    }
}
