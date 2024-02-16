using System.ComponentModel.DataAnnotations;
using Basket.API.Models.Dto;
using Shared;

namespace Basket.API.Models;

public class Basket : IEntityWithSubId<int>
{
    [Key]
    public int Id { get; set; }
    public int SubId { get; set; }
    public Dictionary<int, int>? Products { get; set; }
    
    public double TotalCount { get; set; }
    
}