using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models.Dto;

public class ShoppingCartDto
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;
    
    public int ProductId { get; set; }

    public ProductDto Product { get; set; } = null!;
    
    public int OrderId { get; set; }
}

