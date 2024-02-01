using Basket.API.Models;
using Basket.API.Models.Dto;

namespace Basket.API.Services.Interfaces;

public interface IBasketService
{
    Task<ShoppingCartDto> CreateShoppingCart(string login, int productId);
    Task<IEnumerable<ShoppingCartDto>> GetProductsIntoBasket(string login);
    
    
}