using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectPRN221_Supermarket.Models
{
    public partial class SupermarketDBContext : DbContext
    {
        public SupermarketDBContext()
        {
        }

        public SupermarketDBContext(DbContextOptions<SupermarketDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }
        public virtual DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
				var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
				optionsBuilder.UseSqlServer(config.GetConnectionString("MyDB"));
			}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("categoryName");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Category");
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Purchase__C3905BAFB6C936B1");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__PurchaseO__Suppl__286302EC");
            });

            modelBuilder.Entity<PurchaseOrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId)
                    .HasName("PK__Purchase__57ED06A13778DC6F");

                entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PurchaseOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__PurchaseO__Order__2B3F6F97");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PurchaseOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__PurchaseO__Produ__2C3393D0");
            });

            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__SalesOrd__C3905BAFE7179193");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.OrderDate).HasColumnType("date");
            });

            modelBuilder.Entity<SalesOrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId)
                    .HasName("PK__SalesOrd__57ED06A13731A5B9");

                entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.SalesOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__SalesOrde__Order__30F848ED");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesOrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__SalesOrde__Produ__31EC6D26");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
