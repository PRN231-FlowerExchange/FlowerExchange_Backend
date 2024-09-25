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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(u => u.Id);

            // FlowerOrder one to many Transaction
            builder
                .HasOne(t => t.FlowerOrder)
                .WithMany(fo => fo.Transactions)
                .HasForeignKey(t => t.FlowerOrderId);

            // ServiceOrder one to many Transaction
            builder
                .HasOne(t => t.ServiceOrder)
                .WithMany(so => so.Transactions)
                .HasForeignKey(t => t.ServiceOrderId);
        }
    }
}
