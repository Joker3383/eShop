namespace MVC.Controllers;

public class ProductRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
    public double Price { get; set; }

    [Required(ErrorMessage = "CategoryName is required")]
    public string CategoryName { get; set; }

    public string? Description { get; set; }

    [Url(ErrorMessage = "ImageUrl must be a valid URL")]
    public string? ImageUrl { get; set; }
}