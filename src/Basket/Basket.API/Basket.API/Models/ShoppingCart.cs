using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Basket.API.Models.Dto;

namespace Basket.API.Models;

public class ShoppingCart
{
    [Key]
    public int Id { get; set; }

    public string Login { get; set; } = null!;
    
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }

    public ProductDto Product { get; set; } = null!;
}