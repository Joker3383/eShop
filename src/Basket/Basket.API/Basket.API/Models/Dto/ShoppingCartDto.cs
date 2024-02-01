namespace Basket.API.Models.Dto;

public class ShoppingCartDto
{

    public int Id { get; set; }

    public string Login { get; set; } = null!;
    
    public int ProductId { get; set; }

    public ProductDto Product { get; set; } = null!;
}