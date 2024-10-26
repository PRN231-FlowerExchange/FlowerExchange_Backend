using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransaction");

            builder.HasKey(wt => wt.Id);

            //Wallet many to many Transaction => WalletTransaction 
            //builder
            //    .HasKey(wt => new { wt.WalletId, wt.TransactonId });

            builder
                .HasOne(wt => wt.Wallet)
                .WithMany(w => w.WalletTransactions)
                .HasForeignKey(wt => wt.WalletId);

            builder
                .HasOne(wt => wt.Transaction)
                .WithMany(t => t.WalletTransactions)
                .HasForeignKey(wt => wt.TransactonId);


        }
    }
}
