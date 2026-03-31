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
    public class PartnerConfiguration
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.ToTable("Partners", "app");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.INN)
                .HasMaxLength(20);

            builder.Property(p => p.Phone)
                .HasMaxLength(20);

            builder.Property(p => p.Email)
                .HasMaxLength(100);

            builder.Property(p => p.DirectorName)
                .HasMaxLength(200);

            builder.Property(p => p.LogoPath)
                .HasMaxLength(500);

            builder.Property(p => p.Rating)
                .HasDefaultValue(0);

            builder.HasOne(p => p.PartnerType)
                .WithMany(pt => pt.Partners)
                .HasForeignKey(p => p.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
