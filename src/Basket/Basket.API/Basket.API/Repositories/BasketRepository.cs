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

    


    public async Task<Models.Basket?> GetBasket(int userId)
    {
        return await _appDbContext.Set<Models.Basket>().FirstOrDefaultAsync(b => b.SubId == userId);
    }
}