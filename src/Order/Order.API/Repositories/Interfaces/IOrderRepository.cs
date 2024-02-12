using Order.API.Models.Dto;

namespace Order.API.Repositories.Interfaces;

public interface IOrderRepository
{
    IQueryable<Models.Order> FindAll(int subId);
}