using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class FlowerOrderConfiguration : IEntityTypeConfiguration<FlowerOrder>
    {
        public void Configure(EntityTypeBuilder<FlowerOrder> builder)
        {
            builder.ToTable("FlowerOrder");

            builder.HasKey(u => u.Id);
            // Buyer (User) one to many FlowerOrder
            builder
                .HasOne(fo => fo.Buyer)
                .WithMany(b => b.BuyOrders)
                .HasForeignKey(fo => fo.BuyerId);

            // Seller (User) one to many FlowerOrder
            builder
                .HasOne(fo => fo.Seller)
                .WithMany(s => s.SellOrders)
                .HasForeignKey(fo => fo.SellerId);
        }
    }
}
