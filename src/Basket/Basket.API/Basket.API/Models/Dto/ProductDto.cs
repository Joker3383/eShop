using System.ComponentModel.DataAnnotations;

namespace Basket.API.Models.Dto;

public class ProductDto
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string CategoryName { get; set; }= null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}