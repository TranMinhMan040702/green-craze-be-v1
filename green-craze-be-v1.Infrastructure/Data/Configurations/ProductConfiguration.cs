using green_craze_be_v1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasAlternateKey(x => x.Name);
            builder.HasAlternateKey(x => x.Slug);
            builder.HasAlternateKey(x => x.Code);
            builder.Property(x => x.Cost).HasColumnType("DECIMAL").IsRequired();
            builder.Property(x => x.ShortDescription).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Sold).IsRequired();
            builder.Property(x => x.Rating).IsRequired();
            builder.Property(x => x.Slug).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }
}