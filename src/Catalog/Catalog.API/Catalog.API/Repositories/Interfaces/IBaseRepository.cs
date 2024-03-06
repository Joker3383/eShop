namespace Catalog.API.Repositories.Interfaces;

public interface IBaseRepository
{
    Task<Product> Create(Product entity);
    Task<Product> Update(Product entity);
    Task<Product> Delete(Product entity);
    IQueryable<Product> FindAll();
    Task<Product?> FindById(int id);
}