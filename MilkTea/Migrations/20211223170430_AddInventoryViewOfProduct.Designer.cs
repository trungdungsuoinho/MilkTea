﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkTea.Entities;

namespace MilkTea.Migrations
{
    [DbContext(typeof(MilkTeaContext))]
    [Migration("20211223170430_AddInventoryViewOfProduct")]
    partial class AddInventoryViewOfProduct
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MilkTea.Entities.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("MilkTea.Entities.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("TotolPrice")
                        .HasColumnType("int");

                    b.HasKey("CartId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("MilkTea.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Enable")
                        .HasColumnType("bit");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MilkTea.Entities.Dish", b =>
                {
                    b.Property<int>("DishId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DishPrice")
                        .HasColumnType("int");

                    b.Property<int>("Ice")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantily")
                        .HasColumnType("int");

                    b.Property<string>("SizeName")
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("Sugar")
                        .HasColumnType("int");

                    b.Property<int?>("ToppingId")
                        .HasColumnType("int");

                    b.HasKey("DishId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeName");

                    b.HasIndex("ToppingId");

                    b.ToTable("Dishes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Dish");
                });

            modelBuilder.Entity("MilkTea.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReceiveDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiveId")
                        .HasColumnType("int");

                    b.Property<int>("ShipPrice")
                        .HasColumnType("int");

                    b.Property<int>("TotolPrice")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("ReceiveId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MilkTea.Entities.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Inventory")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("View")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MilkTea.Entities.Receive", b =>
                {
                    b.Property<int>("ReceiveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ReceiveAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiveName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiveNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceivePhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReceiveId");

                    b.HasIndex("UserId");

                    b.ToTable("Receives");
                });

            modelBuilder.Entity("MilkTea.Entities.Size", b =>
                {
                    b.Property<string>("SizeName")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("SizeName");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("MilkTea.Entities.Topping", b =>
                {
                    b.Property<int>("ToppingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ToppingName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ToppingId");

                    b.ToTable("Toppings");
                });

            modelBuilder.Entity("MilkTea.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MilkTea.Entities.DishCart", b =>
                {
                    b.HasBaseType("MilkTea.Entities.Dish");

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.HasIndex("CartId");

                    b.HasDiscriminator().HasValue("DishCart");
                });

            modelBuilder.Entity("MilkTea.Entities.DishOrder", b =>
                {
                    b.HasBaseType("MilkTea.Entities.Dish");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasIndex("OrderId");

                    b.HasDiscriminator().HasValue("DishOrder");
                });

            modelBuilder.Entity("MilkTea.Entities.Cart", b =>
                {
                    b.HasOne("MilkTea.Entities.User", "User")
                        .WithOne("Cart")
                        .HasForeignKey("MilkTea.Entities.Cart", "CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MilkTea.Entities.Dish", b =>
                {
                    b.HasOne("MilkTea.Entities.Product", "Product")
                        .WithMany("Dishs")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilkTea.Entities.Size", "Size")
                        .WithMany("Dishs")
                        .HasForeignKey("SizeName");

                    b.HasOne("MilkTea.Entities.Topping", "Topping")
                        .WithMany("Dishs")
                        .HasForeignKey("ToppingId");

                    b.Navigation("Product");

                    b.Navigation("Size");

                    b.Navigation("Topping");
                });

            modelBuilder.Entity("MilkTea.Entities.Order", b =>
                {
                    b.HasOne("MilkTea.Entities.Receive", "Receive")
                        .WithMany("Orders")
                        .HasForeignKey("ReceiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MilkTea.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receive");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MilkTea.Entities.Product", b =>
                {
                    b.HasOne("MilkTea.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MilkTea.Entities.Receive", b =>
                {
                    b.HasOne("MilkTea.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MilkTea.Entities.DishCart", b =>
                {
                    b.HasOne("MilkTea.Entities.Cart", "Cart")
                        .WithMany("DishCarts")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("MilkTea.Entities.DishOrder", b =>
                {
                    b.HasOne("MilkTea.Entities.Order", "Order")
                        .WithMany("DishOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("MilkTea.Entities.Cart", b =>
                {
                    b.Navigation("DishCarts");
                });

            modelBuilder.Entity("MilkTea.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("MilkTea.Entities.Order", b =>
                {
                    b.Navigation("DishOrders");
                });

            modelBuilder.Entity("MilkTea.Entities.Product", b =>
                {
                    b.Navigation("Dishs");
                });

            modelBuilder.Entity("MilkTea.Entities.Receive", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("MilkTea.Entities.Size", b =>
                {
                    b.Navigation("Dishs");
                });

            modelBuilder.Entity("MilkTea.Entities.Topping", b =>
                {
                    b.Navigation("Dishs");
                });

            modelBuilder.Entity("MilkTea.Entities.User", b =>
                {
                    b.Navigation("Cart");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
