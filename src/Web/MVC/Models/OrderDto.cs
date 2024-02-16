namespace MVC.Models;

public class OrderDto
{
    public int Id { get; set; }
    public string SubId { get; set; } = null!;
    public double TotalSum { get; set; }
    public DateTime DateOfOrder { get; set; }
}