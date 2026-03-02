using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Warehouse.Features.Account;

namespace Warehouse.Infrastructure.Data;

public partial class WarehouseDbContext : IdentityDbContext<ApplicationUser>
{
    public WarehouseDbContext()
    {
    }

    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

   

    public virtual DbSet<Warehouse> Warehouses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Order Configuration ---
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.TotalAmount)
                  .HasColumnType("decimal(18,2)"); // إضافة نوع البيانات لضمان الدقة

            entity.Property(e => e.Status)
                  .HasConversion<int>()
                  .HasDefaultValue(OrderStatus.Pending);

            entity.Property(e => e.IsDeleted)
                  .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("(getutcdate())");

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasMany(o => o.OrderItems)
                  .WithOne(oi => oi.Order)
                  .HasForeignKey(oi => oi.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // --- OrderItem Configuration ---
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UnitPrice)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(oi => oi.Product)
                  .WithMany()
                  .HasForeignKey(oi => oi.ProductId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // --- Product Configuration ---
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // --- Stock Configuration ---
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken()
                  .ValueGeneratedOnAddOrUpdate();

            entity.HasIndex(s => new { s.ProductId, s.WarehouseId })
                  .IsUnique()
                  .HasDatabaseName("UQ_Product_Warehouse");

            entity.HasOne(s => s.Product)
                  .WithMany()
                  .HasForeignKey(s => s.ProductId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Stock_Product");

            // تم التعديل هنا لربط الـ Stocks بداخل كلاس Warehouse صراحةً
            entity.HasOne(s => s.Warehouse)
                  .WithMany(w => w.Stocks)
                  .HasForeignKey(s => s.WarehouseId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Stock_Warehouse");

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // --- Warehouse Configuration ---
        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.Property(e => e.Location).HasMaxLength(300);

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}