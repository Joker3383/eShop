using Shared;

namespace Basket.API.Models;

public class Product : IEntity<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double Price { get; set; }
    public required string CategoryName { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}