using Basket.API.Models;

namespace Basket.API.Repositories.Interfaces;

public interface IBasketRepository
{
    Task<Models.Basket> CreateBasketAsync(Models.Basket basket);
    Task<Models.Basket> DeleteBasketAsync(Models.Basket basket );
    Task<Models.Basket> UpdateBasketAsync(Models.Basket basket);
    
    Task<Models.Basket?> GetBasket(int subId);
    
    
    
    
    
}