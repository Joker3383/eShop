using Catalog.API.Data;
using Catalog.API.Models.Dto;

namespace Catalog.API.Services.Interfaces;

public interface IProductService
{
    Task<Product> CreateProduct(ProductDto productDto);
    Task<Product> UpdateProduct(ProductDto productDto);
    Task<Product> DaleteProduct(int id);
    Task<IQueryable<Product>> GetProducts();
    Task<Product?> GetProductById(int id);
}