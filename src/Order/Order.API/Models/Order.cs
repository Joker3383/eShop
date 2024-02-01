using System.ComponentModel.DataAnnotations;
using Order.API.Models.Dto;

namespace Order.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public decimal TotalSum { get; set; }
        public DateTime DateOfOrder { get; set; }
        public ICollection<ShoppingCartDto> ShoppingCarts { get; set; } = new List<ShoppingCartDto>();
    }
}

