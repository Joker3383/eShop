// using Basket.API.Data.Configurations;
using Basket.API.Models;
using Basket.API.Models.Dto;
using Microsoft.EntityFrameworkCore;


namespace Basket.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
    
    public DbSet<Models.Basket> Baskets { get; set; }
    public DbSet<Models.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}