using System.ComponentModel.DataAnnotations;
using Basket.API.Models.Dto;
using Shared;

namespace Basket.API.Models;

public class Basket : IEntity<int>
{
    [Key]
    public int Id { get; set; }
    public int SubId { get; set; }
    // product id - quantity
    public Dictionary<int, int>? Products { get; set; }
    
}