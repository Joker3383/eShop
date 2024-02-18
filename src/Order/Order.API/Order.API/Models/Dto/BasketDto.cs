namespace Order.API.Models.Dto;

public class BasketDto
{
    public int Id { get; set; }
    public int SubId { get; set; }

    public Dictionary<int, int>? Products { get; set; }
    public double TotalCount { get; set; }
}