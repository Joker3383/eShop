using Basket.API.Models.Dto;

namespace Basket.API.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetProducts();
}