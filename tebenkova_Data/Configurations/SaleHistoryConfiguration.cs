using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tebenkova_Models.Models;

namespace tebenkova_Data.Configurations
{
    public class SaleHistoryConfiguration : IEntityTypeConfiguration<SaleHistory>
    {
        public void Configure(EntityTypeBuilder<SaleHistory> builder)
        {
            builder.ToTable("SalesHistories", "app");

            builder.HasKey(sh => sh.Id);

            builder.Property(sh => sh.SaleDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(sh => sh.Quantity)
                .IsRequired();

            builder.Property(sh => sh.SalePrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(sh => sh.Partner)
                .WithMany(p => p.SalesHistories)
                .HasForeignKey(sh => sh.PartnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sh => sh.Product)
                .WithMany(p => p.SalesHistories)
                .HasForeignKey(sh => sh.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
