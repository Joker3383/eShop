using MVC.Models;

namespace MVC.Services.Interfaces;

public interface IBasketService
{
    Task<ResponseDto?> AddProductIntoBasketAsync(string subjectId, int productId, int quantity);

    Task<ResponseDto?> GetBasketAsync(string subjectId);

    Task<ResponseDto?> DeleteProductFromBasketAsync(string subjectId, int productId, int quantity);

    Task<ResponseDto?> CreateBasketAsync(string subjectId);

    Task<ResponseDto?> DeleteBasketAsync(string subjectId);
}