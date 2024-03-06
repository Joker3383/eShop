namespace Catalog.API.Repositories;

public class BaseRepository : IBaseRepository
{
    private readonly AppDbContext _appDbContext;
    internal ILogger<BaseRepository> _logger;

    public BaseRepository(AppDbContext appDbContext, ILogger<BaseRepository> logger)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }
    
    public async Task<Product> Create(Product entity)
    {
        await _appDbContext.Set<Product>().AddAsync(entity);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 0)
        {
            throw new RepositoryException("Entity didn`t save changes!");
        }
        return entity;
    }

    public async Task<Product> Update(Product entity)
    {
        _appDbContext.Set<Product>().Update(entity);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 0)
        {
            throw new RepositoryException("Product didn`t save changes!");
        }
        return entity;
    }

    public async Task<Product> Delete(Product entity)
    {
        _appDbContext.Set<Product>().Remove(entity);
        var result = await _appDbContext.SaveChangesAsync();
        if (result == 0)
        {
            throw new RepositoryException("Product didn`t save changes!");
        }
        return entity;

    }

    public IQueryable<Product> FindAll()
    {
        var result = _appDbContext.Set<Product>().AsQueryable();
        if (result == null)
        {
            throw new RepositoryException("There are no Products");
        }

        return result;
    }

    public async Task<Product?> FindById(int id)
    {
        var result = await _appDbContext.Set<Product>().FindAsync(id);
        if (result == null)
        {
            throw new RepositoryException($"Not Found Product By id: {id}!");
        }

        return result;
    }
}