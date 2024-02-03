using System.ComponentModel.DataAnnotations;
using Order.API.Models.Dto;

namespace Order.API.Models
{
    public class Order
    {

        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public double TotalSum { get; set; }
        public DateTime DateOfOrder { get; set; }
        public ICollection<ShoppingCartDto> ShoppingCarts { get; set; } 
    }
}

