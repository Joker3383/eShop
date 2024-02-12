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


    public async Task<Models.Basket> CreateBasketAsync(Models.Basket basket)
    {
        var addedBasket = await _appDbContext.Baskets.AddAsync(basket);

        var result = await _appDbContext.SaveChangesAsync();

        if (result == 0)
        {
            throw new RepositoryException("Basket didn`t add");
        }

        return addedBasket.Entity;
    }

    public async Task<Models.Basket> DeleteBasketAsync(Models.Basket basket)
    {
        var addedBasket =  _appDbContext.Baskets.Remove(basket);

        var result = await _appDbContext.SaveChangesAsync();

        if (result == 0)
        {
            throw new RepositoryException("Basket didn`t delete");
        }

        return addedBasket.Entity;
    }

    public async Task<Models.Basket> UpdateBasketAsync(Models.Basket basket)
    {
        var addedBasket =  _appDbContext.Baskets.Update(basket);

        var result = await _appDbContext.SaveChangesAsync();

        if (result == 0)
        {
            throw new RepositoryException("Basket didn`t change");
        }

        return addedBasket.Entity;
    }


    public async Task<Models.Basket?> GetBasket(int userId)
    {
        return await _appDbContext.Set<Models.Basket>().FirstOrDefaultAsync(b => b.SubId == userId);
    }
}