using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Repositories.Interfaces;

namespace Order.API.Repositories;

public class OrderRepository : IOrderRepository
{
    private AppDbContext _appDbContext;

    public OrderRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<Models.Order> CreateOrder(Models.Order order)
    {
        foreach (var shoppingCart in order.ShoppingCarts)
        {

            var existingProduct =  _appDbContext.Products
                .FirstOrDefault(p => p.ProductId == shoppingCart.ProductId);

            if (existingProduct == null)
            {
                _appDbContext.Orders.Add(order);
                var res = await _appDbContext.SaveChangesAsync();
                if (res == 0)
                {
                    throw new RepositoryException("Order didn`t save changes!");
                }
                return order;
            }
            
            _appDbContext.Attach(existingProduct);
            
            shoppingCart.Product = existingProduct;
        }
        _appDbContext.Orders.Add(order);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 0)
        {
            throw new RepositoryException("Order didn`t save changes!");
        }
        return order;
    
    }

    public IQueryable<Models.Order> FindAll()
    {
        var result = _appDbContext.Set<Models.Order>().AsQueryable().AsNoTracking()
            .Include(p => p.ShoppingCarts)
            .ThenInclude(sh => sh.Product);
        if (result == null)
        {
            throw new RepositoryException("There are no Orders");
        }

        return result;
    }
}