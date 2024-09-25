﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
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

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Conversation", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.DepositTransaction", b =>
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

            modelBuilder.Entity("Domain.Entities.Flower", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
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

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("Flower", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.FlowerOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FlowerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsRefund")
                        .HasColumnType("boolean");

                    b.Property<Guid>("SellerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.HasIndex("FlowerId")
                        .IsUnique();

                    b.HasIndex("SellerId");

                    b.ToTable("FlowerOrder", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("SenderId");

                    b.ToTable("Message", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
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

                    b.Property<Guid?>("SellerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("StoreId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UnitMeasure")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.HasIndex("StoreId");

                    b.ToTable("Post", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.PostCategory", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("PostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PostCategory", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.PostService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ServiceOrderId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("ServiceId");

                    b.ToTable("PostService", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("reportByUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("reportByUserId");

                    b.ToTable("Report", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Service", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.ServiceOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.ToTable("ServiceOrder", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Store", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTimeOffset?>("CreatedAt")
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

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FlowerOrderId");

                    b.HasIndex("ServiceOrderId");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
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

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.UserConversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserConversation", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Currency")
                        .HasColumnType("integer");

                    b.Property<double>("TotalBalance")
                        .HasColumnType("double precision");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Wallet", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.WalletTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TransactonId")
                        .HasColumnType("uuid");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TransactonId");

                    b.HasIndex("WalletId");

                    b.ToTable("WalletTransaction", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.WeatherForecast", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.Property<int>("TemperatureC")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("createById")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("updateById")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("WeatherForecast", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.DepositTransaction", b =>
                {
                    b.HasOne("Domain.Entities.Wallet", "Wallet")
                        .WithMany("DepositTransactions")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Domain.Entities.Flower", b =>
                {
                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithOne("Flower")
                        .HasForeignKey("Domain.Entities.Flower", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.FlowerOrder", b =>
                {
                    b.HasOne("Domain.Entities.User", "Buyer")
                        .WithMany("BuyOrders")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Flower", "Flower")
                        .WithOne("FlowerOrder")
                        .HasForeignKey("Domain.Entities.FlowerOrder", "FlowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "Seller")
                        .WithMany("SellOrders")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");

                    b.Navigation("Flower");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("Domain.Entities.Message", b =>
                {
                    b.HasOne("Domain.Entities.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "Sender")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.HasOne("Domain.Entities.User", "Seller")
                        .WithMany("Posts")
                        .HasForeignKey("SellerId");

                    b.HasOne("Domain.Entities.Store", "Store")
                        .WithMany("Posts")
                        .HasForeignKey("StoreId");

                    b.Navigation("Seller");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Domain.Entities.PostCategory", b =>
                {
                    b.HasOne("Domain.Entities.Category", "Category")
                        .WithMany("PostCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithMany("PostCategories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.PostService", b =>
                {
                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithMany("PostServices")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Service", "Service")
                        .WithMany("PostServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.ServiceOrder", "ServiceOrder")
                        .WithMany("PostServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Service");

                    b.Navigation("ServiceOrder");
                });

            modelBuilder.Entity("Domain.Entities.Report", b =>
                {
                    b.HasOne("Domain.Entities.Post", "Post")
                        .WithMany("Reports")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "reportBy")
                        .WithMany("Reports")
                        .HasForeignKey("reportByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("reportBy");
                });

            modelBuilder.Entity("Domain.Entities.ServiceOrder", b =>
                {
                    b.HasOne("Domain.Entities.User", "Buyer")
                        .WithMany("ServiceOrders")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buyer");
                });

            modelBuilder.Entity("Domain.Entities.Store", b =>
                {
                    b.HasOne("Domain.Entities.User", "Owner")
                        .WithOne("Store")
                        .HasForeignKey("Domain.Entities.Store", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Domain.Entities.Transaction", b =>
                {
                    b.HasOne("Domain.Entities.FlowerOrder", "FlowerOrder")
                        .WithMany("Transactions")
                        .HasForeignKey("FlowerOrderId");

                    b.HasOne("Domain.Entities.ServiceOrder", "ServiceOrder")
                        .WithMany("Transactions")
                        .HasForeignKey("ServiceOrderId");

                    b.Navigation("FlowerOrder");

                    b.Navigation("ServiceOrder");
                });

            modelBuilder.Entity("Domain.Entities.UserConversation", b =>
                {
                    b.HasOne("Domain.Entities.Conversation", "Conversation")
                        .WithMany("UserConversations")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("UserConversations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Wallet", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("Domain.Entities.Wallet", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.WalletTransaction", b =>
                {
                    b.HasOne("Domain.Entities.Transaction", "Transaction")
                        .WithMany("WalletTransactions")
                        .HasForeignKey("TransactonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Wallet", "Wallet")
                        .WithMany("WalletTransactions")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Navigation("PostCategories");
                });

            modelBuilder.Entity("Domain.Entities.Conversation", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UserConversations");
                });

            modelBuilder.Entity("Domain.Entities.Flower", b =>
                {
                    b.Navigation("FlowerOrder")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.FlowerOrder", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Domain.Entities.Post", b =>
                {
                    b.Navigation("Flower")
                        .IsRequired();

                    b.Navigation("PostCategories");

                    b.Navigation("PostServices");

                    b.Navigation("Reports");
                });

            modelBuilder.Entity("Domain.Entities.Service", b =>
                {
                    b.Navigation("PostServices");
                });

            modelBuilder.Entity("Domain.Entities.ServiceOrder", b =>
                {
                    b.Navigation("PostServices");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Domain.Entities.Store", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("Domain.Entities.Transaction", b =>
                {
                    b.Navigation("WalletTransactions");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("BuyOrders");

                    b.Navigation("Messages");

                    b.Navigation("Posts");

                    b.Navigation("Reports");

                    b.Navigation("SellOrders");

                    b.Navigation("ServiceOrders");

                    b.Navigation("Store");

                    b.Navigation("UserConversations");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Domain.Entities.Wallet", b =>
                {
                    b.Navigation("DepositTransactions");

                    b.Navigation("WalletTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
