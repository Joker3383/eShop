using Basket.API.Models;

namespace Basket.API.Repositories.Interfaces;

public interface IBasketRepository
{
    Task<ShoppingCart> Create(ShoppingCart cart);
    Task<ShoppingCart> Delete(ShoppingCart cart);
    IQueryable<ShoppingCart> FindAll();
}