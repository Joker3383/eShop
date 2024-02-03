using Order.API.Models.Dto;

namespace Order.API.Repositories.Interfaces;

public interface IBasketRepository
{
    Task<ICollection<ShoppingCartDto>> GetShoppingCarts(string login);
}