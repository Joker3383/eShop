namespace Basket.API.Models.Dto;

public class BasketDto
{

    public int Id { get; set; }
    public int SubId { get; set; }
    // product id - quantity
    public Dictionary<int, int>? Products { get; set; }
}