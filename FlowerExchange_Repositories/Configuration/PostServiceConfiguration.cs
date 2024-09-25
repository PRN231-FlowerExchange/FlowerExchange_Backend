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
    public class PostServiceConfiguration : IEntityTypeConfiguration<PostService>
    {
        public void Configure(EntityTypeBuilder<PostService> builder)
        {
            builder.ToTable("PostService");

            builder.HasKey(u => u.Id);

            // Post many to many Service => PostService
            builder
                .HasOne(ps => ps.Post)
                .WithMany(p => p.PostServices)
                .HasForeignKey(ps => ps.PostId);

            builder
                .HasOne(ps => ps.Service)
                .WithMany(s => s.PostServices)
                .HasForeignKey(ps => ps.ServiceId);

            // ServiceOrder one to many PostService
            builder
                .HasOne(po => po.ServiceOrder)
                .WithMany(so => so.PostServices)
                .HasForeignKey(po => po.ServiceId);
        }
    }
}
