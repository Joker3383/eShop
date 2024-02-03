using Basket.API.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.API.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductDto>
{
    public void Configure(EntityTypeBuilder<ProductDto> builder)
    {
        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.ProductId).UseIdentityColumn();
        builder.Property(p => p.Description);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Price);
        builder.Property(p => p.CategoryName).IsRequired();
        builder.Property(p => p.ImageUrl);
    }
}