using Basket.API.Models;
using Microsoft.EntityFrameworkCore;


namespace Basket.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
}