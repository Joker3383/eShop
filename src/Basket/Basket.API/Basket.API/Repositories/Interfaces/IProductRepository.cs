
namespace Basket.API.Repositories.Interfaces;

public interface IProductRepository
{
    Task<ProductDto?> GetProductById(int productId);
}