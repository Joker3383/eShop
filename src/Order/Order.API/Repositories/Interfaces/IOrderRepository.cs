using Order.API.Models.Dto;

namespace Order.API.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Models.Order> CreateOrder(Models.Order order);
    IQueryable<Models.Order> FindAll();
}