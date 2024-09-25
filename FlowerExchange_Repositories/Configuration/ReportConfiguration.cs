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
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Report");

            builder.HasKey(u => u.Id);

            // Post one to many Report 
            builder
                .HasOne(r => r.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => r.PostId);

            // User one to many Report (Create)
            builder
                .HasOne(r => r.reportBy)
                .WithMany(cb => cb.Reports)
                .HasForeignKey(r => r.reportByUserId);

        }
    }
}
