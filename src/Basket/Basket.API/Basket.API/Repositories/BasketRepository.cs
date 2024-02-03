using Basket.API.Data;
using Basket.API.Exceptions;
using Basket.API.Models;
using Basket.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private AppDbContext _appDbContext;

    public BasketRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<ShoppingCart> Create(ShoppingCart cart)
    {
        var existingProduct =   _appDbContext.Products
            .FirstOrDefault(p => p.ProductId == cart.ProductId);

        if (existingProduct == null)
        {
            await _appDbContext.Set<ShoppingCart>().AddAsync(cart);
            var resultWithoutAttach = await _appDbContext.SaveChangesAsync();
            if (resultWithoutAttach == 0)
            {
                throw new RepositoryException("Entity didn`t save changes!");
            }
            return cart;
        }
        
        _appDbContext.Attach(existingProduct);
        
        cart.Product = existingProduct;
        
        await _appDbContext.Set<ShoppingCart>().AddAsync(cart);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 0)
        {
            throw new RepositoryException("Entity didn`t save changes!");
        }
        return cart;
    }

    public async Task<ShoppingCart> Delete(ShoppingCart cart)
    {
        _appDbContext.Set<ShoppingCart>().Remove(cart);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 0)
        {
            throw new RepositoryException("Product didn`t save changes!");
        }
        return cart;
    }

    public IQueryable<ShoppingCart> FindAll()
    {
        var result = _appDbContext.Set<ShoppingCart>().AsQueryable().AsNoTracking().Include(p => p.Product);
        if (result == null)
        {
            throw new RepositoryException("There are no Products");
        }

        return result;
    }
}