using AuthApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApp.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(t => t.Name)
         .HasMaxLength(100)
         .IsRequired();

        builder.Property(t => t.Description)
             .HasMaxLength(400)
             .IsRequired();

        builder.Property(t => t.Price)
             .IsRequired();
    }
}
