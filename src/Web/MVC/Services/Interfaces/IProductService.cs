
using MVC.Models;

namespace MVC.Services.Interfaces;

public interface IProductService
{
     
    Task<ResponseDto?> GetAllProductsAsync();
    Task<ResponseDto?> GetProductByIdAsync(int id);
    Task<ResponseDto?> CreateProductsAsync(ProductDto productDto);
    Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto);
    Task<ResponseDto?> DeleteProductsAsync(int id);
}