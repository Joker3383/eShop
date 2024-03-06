namespace MVC.Models;

public class OrderDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "SubId is required")]
    public string SubId { get; set; } = null!;

    [Required(ErrorMessage = "TotalSum is required")]
    [Range(0, double.MaxValue, ErrorMessage = "TotalSum must be greater than or equal to 0")]
    public double TotalSum { get; set; }

    [Required(ErrorMessage = "DateOfOrder is required")]
    [DataType(DataType.DateTime, ErrorMessage = "DateOfOrder must be a valid DateTime")]
    public DateTime DateOfOrder { get; set; }
}