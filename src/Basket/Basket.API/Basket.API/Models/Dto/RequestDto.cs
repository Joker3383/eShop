
namespace Basket.API.Models.Dto;

public class RequestDto
{
    [Required(ErrorMessage = "SubId is required")]
    public int SubId { get; set; }

    [Required(ErrorMessage = "ProductId is required")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}