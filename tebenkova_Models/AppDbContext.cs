using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;

namespace tebenkova_Models
{
    public class AppDbContext : DbContext
    {
        // DbSet для всех таблиц
        public DbSet<PartnerType> PartnerTypes { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SaleHistory> SalesHistories { get; set; }
        public DbSet<StockReceipt> StockReceipts { get; set; }
        public DbSet<StockIssue> StockIssues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tebenkova;Username=app;Password=123456789");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Устанавливаем схему по умолчанию
            modelBuilder.HasDefaultSchema("app");

            // Настройка связей для Partner
            modelBuilder.Entity<Partner>()
                .HasOne(p => p.PartnerType)
                .WithMany(pt => pt.Partners)
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связей для Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связей для SaleHistory
            modelBuilder.Entity<SaleHistory>()
                .HasOne(sh => sh.Partner)
                .WithMany(p => p.SalesHistories)
                .HasForeignKey(sh => sh.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleHistory>()
                .HasOne(sh => sh.Product)
                .WithMany(p => p.SalesHistories)
                .HasForeignKey(sh => sh.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связей для StockReceipt
            modelBuilder.Entity<StockReceipt>()
                .HasOne(sr => sr.Product)
                .WithMany(p => p.StockReceipts)
                .HasForeignKey(sr => sr.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockReceipt>()
                .HasOne(sr => sr.Supplier)
                .WithMany(s => s.StockReceipts)
                .HasForeignKey(sr => sr.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка связей для StockIssue
            modelBuilder.Entity<StockIssue>()
                .HasOne(si => si.Product)
                .WithMany(p => p.StockIssues)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
