using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models.Dto;

public class ShoppingCartDto
{
    [Key]
    public int Id { get; set; }

    public string Login { get; set; } = null!;
    
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }

    public ProductDto Product { get; set; } = null!;
}

