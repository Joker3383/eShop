namespace Order.API.Models.Dto;

public class OrderDto
{
    public int Id { get; set; }
    public int SubId { get; set; }
    public double TotalSum { get; set; }
    public DateTime DateOfOrder { get; set; }
    public int BasketId { get; set; }
}