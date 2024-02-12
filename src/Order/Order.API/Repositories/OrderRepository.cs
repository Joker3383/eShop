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
    
    public  IQueryable<Models.Order> FindAll(int subId)
    {
        var result =  _appDbContext.Set<Models.Order>().Where(o => o.SubId == subId);
        if (result == null)
        {
            throw new RepositoryException("There are no Orders");
        }

        return result;
    }
}