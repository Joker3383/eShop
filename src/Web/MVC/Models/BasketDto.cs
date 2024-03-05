using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class BasketDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "SubId is required")]
    public int SubId { get; set; }

    public Dictionary<int, int>? Products { get; set; }

    [Required(ErrorMessage = "TotalCount is required")]
    [Range(0, double.MaxValue, ErrorMessage = "TotalCount must be greater than or equal to 0")]
    public double TotalCount { get; set; }
}