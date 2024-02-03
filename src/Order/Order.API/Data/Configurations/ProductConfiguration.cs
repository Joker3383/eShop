using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Models.Dto;

namespace Order.API.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductDto>
{
    public void Configure(EntityTypeBuilder<ProductDto> builder)
    {
        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.Description);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Price);
        builder.Property(p => p.CategoryName).IsRequired();
        builder.Property(p => p.ImageUrl);
    }
}