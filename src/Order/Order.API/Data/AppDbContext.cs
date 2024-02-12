using Microsoft.EntityFrameworkCore;
using Order.API.Data.Configurations;
using Order.API.Models.Dto;


namespace Order.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Models.Order> Orders { get; set; }
    public DbSet<ProductDto> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
    }
}