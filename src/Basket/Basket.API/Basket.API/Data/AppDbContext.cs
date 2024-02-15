// using Basket.API.Data.Configurations;

using System.Text.Json;
using Basket.API.Models;
using Basket.API.Models.Dto;
using Microsoft.EntityFrameworkCore;



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