using Basket.API.Data.Configurations;
using Basket.API.Models;
using Basket.API.Models.Dto;
using Microsoft.EntityFrameworkCore;


namespace Basket.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ProductDto> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
    }
}