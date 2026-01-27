using Ciandt.Retail.MCP.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Ciandt.Retail.MCP.Data;

public class RetailDbContext : DbContext
{
    public RetailDbContext(DbContextOptions<RetailDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<PromotionEntity> Promotions { get; set; }
    public DbSet<CouponEntity> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customer configurations
        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<CustomerEntity>()
            .HasMany(c => c.Addresses)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CustomerEntity>()
            .HasMany(c => c.Carts)
            .WithOne(c => c.Customer)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CustomerEntity>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cart configurations
        modelBuilder.Entity<CartEntity>()
            .HasMany(c => c.Items)
            .WithOne(i => i.Cart)
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        // Order configurations
        modelBuilder.Entity<OrderEntity>()
            .HasMany(o => o.OrderItems)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderEntity>()
            .HasMany(o => o.Payments)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product configurations
        modelBuilder.Entity<ProductEntity>()
            .HasIndex(p => p.Category);

        modelBuilder.Entity<ProductEntity>()
            .HasIndex(p => p.Brand);

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Products
        modelBuilder.Entity<ProductEntity>().HasData(
            new ProductEntity
            {
                Id = 1,
                Name = "Roteador NetSpeed 500 Dual Band",
                Brand = "TechConnect",
                Category = "Roteadores",
                Price = 299.99m,
                DiscountedPrice = 249.99m,
                ImageUrl = "roteador1.jpg",
                AverageRating = 4.7,
                ReviewCount = 1258,
                InStock = true,
                AvailableColors = "[\"Preto\",\"Branco\"]",
                HasPromotion = true,
                PromoLabel = "Oferta da Semana",
                IsBestSeller = true,
                IsNew = false,
                CreatedAt = DateTime.UtcNow
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Roteador UltraConnect WiFi 6 Mesh",
                Brand = "NetMaster",
                Category = "Roteadores",
                Price = 599.99m,
                ImageUrl = "https://example.com/images/router-ultraconnect-wifi6.jpg",
                AverageRating = 4.9,
                ReviewCount = 358,
                InStock = true,
                AvailableColors = "[\"Preto\"]",
                HasPromotion = false,
                IsBestSeller = false,
                IsNew = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductEntity
            {
                Id = 3,
                Name = "Impressora ColorJet Pro Multifuncional",
                Brand = "PrintMaster",
                Category = "Impressoras",
                Price = 899.99m,
                DiscountedPrice = 799.99m,
                ImageUrl = "ImpressoraColorJetProMultifuncional.jpg",
                AverageRating = 4.3,
                ReviewCount = 562,
                InStock = true,
                AvailableColors = "[\"Preto\",\"Cinza\"]",
                HasPromotion = true,
                PromoLabel = "Frete Grátis",
                IsBestSeller = false,
                IsNew = false,
                CreatedAt = DateTime.UtcNow
            },
            new ProductEntity
            {
                Id = 4,
                Name = "Notebook UltraBook X5 Core i7",
                Brand = "TechPro",
                Category = "Notebooks",
                Price = 4599.99m,
                DiscountedPrice = 4299.99m,
                ImageUrl = "notebook1.jpg",
                AverageRating = 4.8,
                ReviewCount = 1879,
                InStock = true,
                AvailableColors = "[\"Prata\",\"Grafite\"]",
                HasPromotion = true,
                PromoLabel = "Cashback 10%",
                IsBestSeller = true,
                IsNew = false,
                CreatedAt = DateTime.UtcNow
            },
            new ProductEntity
            {
                Id = 5,
                Name = "Tablet TabX Pro 10.5\"",
                Brand = "GalaxyTech",
                Category = "Tablets",
                Price = 2199.99m,
                ImageUrl = "tablet1.jpg",
                AverageRating = 4.5,
                ReviewCount = 892,
                InStock = false,
                AvailableColors = "[\"Preto\",\"Azul\",\"Rosa\"]",
                AvailableSizes = "[\"64GB\",\"128GB\",\"256GB\"]",
                HasPromotion = false,
                IsBestSeller = false,
                IsNew = true,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed Customers
        modelBuilder.Entity<CustomerEntity>().HasData(
            new CustomerEntity
            {
                Id = 1,
                Name = "Marcio Nizzola",
                Email = "marcio.nizzola@ciandt.com",
                Phone = "5511984701979",
                Zip = "13.328-283",
                CreatedAt = DateTime.UtcNow
            },
            new CustomerEntity
            {
                Id = 2,
                Name = "Ricardo Odorczyk",
                Email = "ricardoso@ciandt.com",
                Phone = "5541999089809",
                Zip = "67890",
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed Addresses
        modelBuilder.Entity<AddressEntity>().HasData(
            new AddressEntity
            {
                Id = 1,
                CustomerId = 1,
                Street = "123 Main St",
                City = "New York",
                State = "NY",
                ZipCode = "12345",
                IsDefault = true,
                CreatedAt = DateTime.UtcNow
            },
            new AddressEntity
            {
                Id = 2,
                CustomerId = 2,
                Street = "456 Broadway",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "67890",
                IsDefault = true,
                CreatedAt = DateTime.UtcNow
            },
            new AddressEntity
            {
                Id = 3,
                CustomerId = 2,
                Street = "789 Park Ave",
                City = "Los Angeles",
                State = "CA",
                ZipCode = "67891",
                IsDefault = false,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}