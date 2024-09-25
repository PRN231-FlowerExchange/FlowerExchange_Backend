using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(u => u.Id);

            builder.ToTable("Post");

            // User one to many Post
            builder
                .HasOne(p => p.Seller)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.SellerId);

            // Store one to many Post
            builder
                .HasOne(p => p.Store)
                .WithMany(s => s.Posts)
                .HasForeignKey(p => p.StoreId);

            // Post one to one Flower
            builder
                .HasOne(p => p.Flower)
                .WithOne(f => f.Post)
                .HasForeignKey<Flower>(f => f.PostId);
        }
    }
}
