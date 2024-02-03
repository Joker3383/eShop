using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Models.Dto;

namespace Order.API.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCartDto>
{
    public void Configure(EntityTypeBuilder<ShoppingCartDto> builder)
    {
        builder.HasKey(sh => sh.Id);
        
        builder.HasOne(sc => sc.Product)
            .WithMany()
            .HasForeignKey(sc => sc.ProductId);

        builder.Property(sh => sh.Login).IsRequired();
    }
}