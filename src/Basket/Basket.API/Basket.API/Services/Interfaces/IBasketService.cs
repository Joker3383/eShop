using Basket.API.Models;
using Basket.API.Models.Dto;

namespace Basket.API.Services.Interfaces;

public interface IBasketService
{
    Task<Models.Basket> CreateBasket(int subId);
    Task<int> DeleteBasket(int subId);

    Task<Models.Basket?> RemoveItemFromBasketAsync(int subId, int productId, int quantity);
    Task<Models.Basket> AddItemIntoBasketAsync(int subId, int productId, int quantity);
    
     Task<Models.Basket?> GetBasket(int subId);

}