using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class FlowerConfiguration : IEntityTypeConfiguration<Flower>
    {
        public void Configure(EntityTypeBuilder<Flower> builder)
        {
            builder.ToTable("Flower");

            builder.HasKey(u => u.Id);

            // Flower one to one FlowerOrder
            builder
                .HasOne(f => f.FlowerOrder)
                .WithOne(fo => fo.Flower)
                .HasForeignKey<FlowerOrder>(fo => fo.FlowerId);
        }
    }
}
