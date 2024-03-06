using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Basket.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
    
    public DbSet<Models.Basket> Baskets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Basket>(entity =>
        {
            entity.Property(e => e.Products)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                    v => JsonSerializer.Deserialize<Dictionary<int, int>>(v, new JsonSerializerOptions(JsonSerializerDefaults.General))!);; 
        });
    }
}