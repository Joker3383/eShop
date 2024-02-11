using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Order.API.Data;
using Order.API.Models.Dto;
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
         _appDbContext.Set<Models.Order>().Add(order);
   
        var result = await _appDbContext.SaveChangesAsync();

        if (result == 0)
        {
            throw new RepositoryException("Order didn't save changes!");
        }

        return order;
    }

    
    public async Task<ShoppingCartDto> CreateShoppingCart(ShoppingCartDto cart)
    {
        var existingProduct =   _appDbContext.Products
            .FirstOrDefault(p => p.ProductId == cart.ProductId);

        if (existingProduct == null)
        {
            await _appDbContext.Set<ShoppingCartDto>().AddAsync(cart);
            var resultWithoutAttach = await _appDbContext.SaveChangesAsync();
            if (resultWithoutAttach == 0)
            {
                throw new RepositoryException("Entity didn`t save changes!");
            }
            return cart;
        }
        
        _appDbContext.Attach(existingProduct);
        
        cart.Product = existingProduct;
        
        await _appDbContext.Set<ShoppingCartDto>().AddAsync(cart);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 0)
        {
            throw new RepositoryException("Entity didn`t save changes!");
        }
        return cart;
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