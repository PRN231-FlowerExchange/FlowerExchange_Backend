﻿// <auto-generated />
using System;
using System.Collections.Generic;
using FlowerExchange_Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlowerExchange_Repositories.Migrations
{
    [DbContext(typeof(FlowerExchangeDbContext))]
    partial class FlowerExchangeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.DepositTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("DepositTransaction");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Flower", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("Flower");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.FlowerOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FlowerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsRefund")
                        .HasColumnType("boolean");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("FlowerId")
                        .IsUnique();

                    b.ToTable("FlowerOrder");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<string>>("ImageUrls")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MainImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PostStatus")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UnitMeasure")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.HasIndex("StoreId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.PostCategory", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("PostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.PostService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ServiceOrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("ServiceId");

                    b.ToTable("PostService");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdateBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uuid");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UpdateBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.ServiceOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("ServiceOrder");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Store");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("FlowerOrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FromWallet")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ServiceOrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("ToWallet")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("FlowerOrderId");

                    b.HasIndex("ServiceOrderId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("LoginType")
                        .HasColumnType("integer");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<double>("TotalBalance")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wallet");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.WalletTransaction", b =>
                {
                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TransactonId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("WalletId", "TransactonId");

                    b.HasIndex("TransactonId");

                    b.ToTable("WalletTransaction");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.DepositTransaction", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.Wallet", "Wallet")
                        .WithMany("DepositTransactions")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Flower", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.Post", "Post")
                        .WithOne("Flower")
                        .HasForeignKey("FlowerExchange_Repositories.Entities.Flower", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.FlowerOrder", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.Flower", "Flower")
                        .WithOne("FlowerOrder")
                        .HasForeignKey("FlowerExchange_Repositories.Entities.FlowerOrder", "FlowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flower");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Post", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.User", "Seller")
                        .WithMany("Posts")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlowerExchange_Repositories.Entities.Store", "Store")
                        .WithMany("Posts")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.PostCategory", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.Category", "Category")
                        .WithMany("PostCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlowerExchange_Repositories.Entities.Post", "Post")
                        .WithMany("PostCategories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.PostService", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.Post", "Post")
                        .WithMany("PostServices")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlowerExchange_Repositories.Entities.Service", "Service")
                        .WithMany("PostServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlowerExchange_Repositories.Entities.ServiceOrder", "ServiceOrder")
                        .WithMany("PostServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Service");

                    b.Navigation("ServiceOrder");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Report", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.Post", "Post")
                        .WithMany("Reports")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Store", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.User", "Owner")
                        .WithOne("Store")
                        .HasForeignKey("FlowerExchange_Repositories.Entities.Store", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Transaction", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.FlowerOrder", "FlowerOrder")
                        .WithMany("Transactions")
                        .HasForeignKey("FlowerOrderId");

                    b.HasOne("FlowerExchange_Repositories.Entities.ServiceOrder", "ServiceOrder")
                        .WithMany("Transactions")
                        .HasForeignKey("ServiceOrderId");

                    b.Navigation("FlowerOrder");

                    b.Navigation("ServiceOrder");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Wallet", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("FlowerExchange_Repositories.Entities.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.WalletTransaction", b =>
                {
                    b.HasOne("FlowerExchange_Repositories.Entities.Transaction", "Transaction")
                        .WithMany("WalletTransactions")
                        .HasForeignKey("TransactonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlowerExchange_Repositories.Entities.Wallet", "Wallet")
                        .WithMany("WalletTransactions")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Category", b =>
                {
                    b.Navigation("PostCategories");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Flower", b =>
                {
                    b.Navigation("FlowerOrder")
                        .IsRequired();
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.FlowerOrder", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Post", b =>
                {
                    b.Navigation("Flower")
                        .IsRequired();

                    b.Navigation("PostCategories");

                    b.Navigation("PostServices");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Service", b =>
                {
                    b.Navigation("PostServices");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.ServiceOrder", b =>
                {
                    b.Navigation("PostServices");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Store", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Transaction", b =>
                {
                    b.Navigation("WalletTransactions");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.User", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Store");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("FlowerExchange_Repositories.Entities.Wallet", b =>
                {
                    b.Navigation("DepositTransactions");

                    b.Navigation("WalletTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
