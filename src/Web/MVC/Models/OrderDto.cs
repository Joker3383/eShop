namespace MVC.Models;

public class OrderDto
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public double TotalSum { get; set; }
    public DateTime DateOfOrder { get; set; }
    public ICollection<ShoppingCartDto> ShoppingCarts { get; set; } = new List<ShoppingCartDto>();
}