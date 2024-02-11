using Basket.API.Models.Dto;

namespace Basket.API.Models;

public class Basket
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    // product id - quantity
    public Dictionary<int, int>? Products { get; set; }
}