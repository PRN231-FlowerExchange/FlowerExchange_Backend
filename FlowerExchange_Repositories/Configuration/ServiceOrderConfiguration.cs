﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
    {
        public void Configure(EntityTypeBuilder<ServiceOrder> builder)
        {
            builder.ToTable("ServiceOrder");

            builder.HasKey(u => u.Id);
            //Buyer(User) one to many ServiceOrder
            builder
                 .HasOne(so => so.Buyer)
                 .WithMany(b => b.ServiceOrders)
                 .HasForeignKey(so => so.BuyerId);
        }
    }
}
