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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            // User one to one Store
            builder.HasOne(u => u.Store)
                   .WithOne(s => s.Owner)
                   .HasForeignKey<Store>(s => s.OwnerId);

            // User one to one Wallet
            builder.HasOne(u => u.Wallet)
                   .WithOne(w => w.User)
                   .HasForeignKey<Wallet>(w => w.UserId);



        }
    }
}
