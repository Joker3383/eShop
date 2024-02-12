using Order.API.Models.Dto;

namespace Order.API.Repositories.Interfaces;

public interface IBasketRepository
{
    Task<BasketDto?> GetBasketsAsync(int subId);
}