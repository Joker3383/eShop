using Order.API.Repositories.Interfaces;

namespace Order.API.Repositories;

public class OrderRepository : IOrderRepository
{
    public Task<Models.Order> CreateOrder(Models.Order order)
    {
        
    }

    public IQueryable<Models.Order> FindAll()
    {
        throw new NotImplementedException();
    }
}