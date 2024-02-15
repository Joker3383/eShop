using Basket.API.Models;

namespace Basket.API.Repositories.Interfaces;

public interface IBasketRepository
{
    Task<Models.Basket?> GetBasket(int subId);
}