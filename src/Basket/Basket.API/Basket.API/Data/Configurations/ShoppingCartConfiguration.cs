using Basket.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.API.Data.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(sh => sh.Id);
        
        builder.HasOne(sc => sc.Product)
            .WithMany()
            .HasForeignKey(sc => sc.ProductId);

        builder.Property(sh => sh.Login).IsRequired();
    }
}