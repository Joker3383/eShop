using Microsoft.EntityFrameworkCore;


namespace Order.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Models.Order> Orders { get; set; }
}