using System.ComponentModel.DataAnnotations;
using Shared;

namespace Catalog.API.Data;

public class Product : IEntity<int>
{
    [Key] 
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Range(0, Double.MaxValue)]
    public double Price { get; set; }
    
    public string CategoryName { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
}   