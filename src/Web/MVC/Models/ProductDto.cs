using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class ProductDto
{

    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Range(0, Double.MaxValue)]
    public double Price { get; set; }
    [Required]
    public string CategoryName { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}